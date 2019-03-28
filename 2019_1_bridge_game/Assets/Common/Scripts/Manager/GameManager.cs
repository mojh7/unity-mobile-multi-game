using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* 게임 상태와 씬 등 전반을 관리하는 매니저 클래스
 * 
 * 19.03.18 Scene 흐름
 * logo -> title -> loading -> MainLobby -> Loading -> Room -> Loading -> ingame
 * 
 * loading 중간 중간에 있는 건 언제든지 바뀔 수도 있음.
 * 
 */
public enum GameScene { LOGO, TITLE, LOADING, MAIN_LOBBY, ROOM, IN_GAME, TEMP_LOBBY, TEMP_GAME }

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    #region variables
    //private enum GameState { NOT_STARTED, GAME_OVER, PLAYING, CLEAR, ENDING }
    //public enum GameMode { NORMAL, RUSH }
    

    private static readonly string[] GAME_SCENE = new string[] { "LogoScene", "TitleScene", "LoadingScene", "MainLobbyScene", "RoomScene", "IngameScene", "TempLobbyScene", "TempGameScene" };

    //private GameState gameState = GameState.NOT_STARTED;
    //[SerializeField]
    //private GameMode gameMode = GameMode.NORMAL;
    [SerializeField] private GameScene gameScene = GameScene.LOGO;
    private GameScene nextScene;

    // 새 게임, 로드 게임 구분
    private bool loadsGameData = false;
    #endregion

    #region get / set
    public bool GetLoadsGameData() { return loadsGameData; }
    public GameScene GetGameScene() { return gameScene; }
    //public GameMode GetMode() { return gameMode; }
    public string GetNextScene() { return GAME_SCENE[(int)nextScene]; }

    // 인게임씬에서 바로 시작할 때 설정해줄 디버깅 용
    public void SetGameScene(GameScene gameScene) { this.gameScene = gameScene; }
    public void SetLoadsGameData(bool _loadsGameData) { loadsGameData = _loadsGameData; }
    //public void SetMode(GameMode gameMode) { this.gameMode = gameMode; }
    #endregion

    #region unityFunc
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);

        GameDataManager.Instance.Initialize();
        DatabaseManager.Instance.Initialize();
        BackendUtils.Instance.Initialize();
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
    #endregion

    #region func
    //public bool IsInGame()
    //{
    //    return gameScene == GameScene.IN_GAME || gameScene == GameScene.BOSS_RUSH;
    //}

    public void LoadNextScene()
    {
        gameScene = nextScene;
        SceneManager.LoadScene(GAME_SCENE[(int)nextScene]);
    }

    /// <summary>
    /// 씬 이동
    /// </summary>
    /// <param name="nextScene">이동할 씬</param>
    /// <param name="goThroughLoading">로딩 씬을 거쳐갈 것인가?</param>
    public void LoadNextScene(GameScene nextScene, bool goThroughLoading)
    {
        if (false == goThroughLoading)
        {
            gameScene = nextScene;
            SceneManager.LoadScene(GAME_SCENE[(int)nextScene]);
        }
        else
        {
            this.nextScene = nextScene;
            SceneManager.LoadScene(GAME_SCENE[(int)GameScene.LOADING]);
        }
    }

    //public void LoadInGame()
    //{
    //    //if (!GameDataManager.Instance.isFirst)
    //    //{
    //    //    SceneDataManager.SetNextScene("TutorialScene");
    //    //    SceneManager.LoadScene("LoadingScene");
    //    //    return;
    //    //}
    //    //if (GameMode.NORMAL == gameMode)
    //    //{
    //    //    gameScene = GameScene.IN_GAME;
    //    //    SceneDataManager.SetNextScene("InGameScene");
    //    //}
    //    //else if (GameMode.RUSH == gameMode)
    //    //{
    //    //    gameScene = GameScene.BOSS_RUSH;
    //    //    SceneDataManager.SetNextScene("BossRushScene");
    //    //}
    //    nextScene = "IngameScene";
    //    SceneManager.LoadScene("LoadingScene");
    //}
    //public void LoadTitle()
    //{
    //    gameScene = GameScene.TITLE;
    //    SceneManager.LoadScene("TitleScene");
    //    //SceneDataManager.SetNextScene("TitleScene");
    //    //SceneManager.LoadScene("LoadingScene");
    //}
    //public void LoadLobby()
    //{
    //    gameScene = GameScene.LOBBY;
    //    SceneManager.LoadScene("LobbyScene");
    //}

    //public void GameOver()
    //{
    //    gameState = GameState.GAME_OVER;
    //    GameDataManager.Instance.ResetData(GameDataManager.UserDataType.INGAME);
    //}
    #endregion
}