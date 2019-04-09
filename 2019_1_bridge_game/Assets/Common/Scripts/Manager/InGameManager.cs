using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// 능률 상승 유용한 단축키 모음!
/// <para>라인번호 이동 : Ctrl+G, 라인 자르고 복사 : Ctrl+L, 라인 지우기 : Ctrl+Shift+L</para>
/// <para>주석달기, 해제 : Ctrl+KC, Ctrl+KU</para>
/// <para>숨기고 보이기 | 현재 scope : Ctrl + MM, 선택한 부분 : Ctrl+MH, 전체 : Ctrl+MO, Ctrl+ML</para>
/// <para>선택 영역 소문자 : Ctrl + U, 대문자 : Ctrl + Shift + U</para>
/// <para>해당 줄 바로 복제 : Ctrl + D</para>
/// </summary>
public class VisaulStudioShortcutKey
{
}


// GameManager가 전반적인 관리하고
// 각 씬당 관리하는 manager 둘 듯?

// 맵 생성 -> ui fade in, player, time 시작 하면 될 듯

public class InGameManager : Photon.Pun.MonoBehaviourPunCallbacks
{
    #region Constants
    public const float ASTEROIDS_MIN_SPAWN_TIME = 5.0f;
    public const float ASTEROIDS_MAX_SPAWN_TIME = 10.0f;

    public const float PLAYER_RESPAWN_TIME = 4.0f;

    public const int PLAYER_MAX_LIVES = 3;

    public const string PLAYER_LIVES = "PlayerLives";
    public const string PLAYER_READY = "IsPlayerReady";
    public const string PLAYER_LOADED_LEVEL = "PlayerLoadedLevel";

    public const string RED_TEAM_PLAYER = "RedTeamPlayer";
    public const string BLUE_TEAM_PLAYER = "BlueTeamPlayer";
    #endregion

    #region variables
    public static InGameManager Instance = null;

    public Text InfoText;
    [SerializeField] private Transform redTeamSpawnPoint;
    [SerializeField] private Transform blueTeamSpawnPoint;
    [SerializeField] private Sprite[] emoticonSprites;
    public GameObject[] sheetMusicPrefabs;

    public Text text;

    // 이후 랜덤한 위치를 유동적으로 대입
    [SerializeField] private Transform baseTowns;
    #endregion

    #region get / set

    public Transform GetRedTeamBaseZone() { return redTeamSpawnPoint; }
    public Transform GetBlueTeamBaseZone() { return blueTeamSpawnPoint; }
    public Transform GetBaseTown() { return baseTowns; }
    public static bool IsRedTeam(int playerNumber)
    {
        return (playerNumber % 2) == 0;
    }
    public static Color GetPlayerColorWithTeam(int playerNumber)
    {
        switch (playerNumber % 2)
        {
            case 0: return Color.red;
            case 1: return Color.blue;
            default: return Color.black;
        }
    }

    public Sprite GetEmoticonSprite(UBZ.MultiGame.Owner.CharacterInfo.EmoticonType type)
    {
        return emoticonSprites[(int)type];
    }
    #endregion

    #region unityFunc
    private void Awake()
    {
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
        CountdownTimer.OnGameEndTimerHasExpired += OnGameEndTimerHasExpired;
    }

    public void Start()
    {
        InfoText.text = "Waiting for other players...";
        Hashtable props = new Hashtable
        {
            {PLAYER_LOADED_LEVEL, true}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        PhotonNetwork.LocalPlayer.SetTeam((PhotonNetwork.LocalPlayer.GetPlayerNumber() % 2) == 0? PunTeams.Team.RED : PunTeams.Team.BLUE);
        Debug.Log("Player Number : " + PhotonNetwork.LocalPlayer.GetPlayerNumber() + ", Team : " + PhotonNetwork.LocalPlayer.GetTeam());

        GameObject playerObj = null;
        if (PunTeams.Team.RED == PhotonNetwork.LocalPlayer.GetTeam())
        {
            playerObj = PhotonNetwork.Instantiate("Player", redTeamSpawnPoint.position, Quaternion.Euler(Vector3.zero));
        }
        else if(PunTeams.Team.BLUE == PhotonNetwork.LocalPlayer.GetTeam())
        {
            playerObj = PhotonNetwork.Instantiate("Player", blueTeamSpawnPoint.position, Quaternion.Euler(Vector3.zero));
        }
        playerObj.GetComponent<UBZ.MultiGame.Owner.Player>().Init();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
        CountdownTimer.OnGameEndTimerHasExpired -= OnGameEndTimerHasExpired;
    }
    #endregion

    #region func
    private void StartGame()
    {
        Debug.Log("Timer 다 되고 게임 스타트");
        AudioManager.Instance.PlayMusic(4);
        // TODO : 조이스틱 on, Player 생성, 게임 시작!
        InGameUIManager.Instance.SetControllable(true);

        if (PhotonNetwork.IsMasterClient)
        {
            // StartCoroutine(SpawnAsteroid());
        }
    }

    string temp = string.Empty;

    private void GameOver()
    {
        Debug.Log("게임 끝");
        InGameUIManager.Instance.SetControllable(false);
        AudioManager.Instance.PlaySound("GameEnd", SFXType.COMMON);

        PunTeams.Team teamName = PunTeams.Team.RED;
        int redTeamScore = 0, blueTeamScore = 0;
        List<Player> redTeamPlayers = PunTeams.PlayersPerTeam[teamName];
        foreach (Player player in redTeamPlayers)
        {
            redTeamScore += player.GetScore();
        }
        teamName = PunTeams.Team.BLUE;
        List<Player> blueTeamPlayers = PunTeams.PlayersPerTeam[teamName];
        foreach (Player player in blueTeamPlayers)
        {
            blueTeamScore += player.GetScore();
        }

        if (redTeamScore > blueTeamScore)
        {
            //InfoText.text = "레드팀 승리!!";
            temp = "레드팀 승리!!";
        }
        else if(redTeamScore < blueTeamScore)
        {
            //InfoText.text = "블루팀 승리!!";
            temp = "블루팀 승리!!";
        }
        else
        {
            //InfoText.text = "무승부!!";
            temp = "무승부!!";
        }

        if (PhotonNetwork.IsMasterClient)
        {
            // StartCoroutine(SpawnAsteroid());
        }

        StartCoroutine(EndOfGame("1", 0));
    }

    private bool CheckAllPlayerLoadedLevel()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object playerLoadedLevel;

            if (p.CustomProperties.TryGetValue(PLAYER_LOADED_LEVEL, out playerLoadedLevel))
            {
                if ((bool)playerLoadedLevel)
                {
                    continue;
                }
            }

            return false;
        }

        return true;
    }

    private void CheckEndOfGame()
    {
        bool allDestroyed = true;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object lives;
            if (p.CustomProperties.TryGetValue(PLAYER_LIVES, out lives))
            {
                if ((int)lives > 0)
                {
                    allDestroyed = false;
                    break;
                }
            }
        }

        if (allDestroyed)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StopAllCoroutines();
            }

            string winner = "";
            int score = -1;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.GetScore() > score)
                {
                    winner = p.NickName;
                    score = p.GetScore();
                }
            }

            StartCoroutine(EndOfGame(winner, score));
        }
    }

    private void OnCountdownTimerIsExpired()
    {
        StartGame();
    }

    private void OnGameEndTimerHasExpired()
    {
        GameOver();
    }

    #endregion

    #region punCallbacks

    public override void OnDisconnected(DisconnectCause cause)
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("TempLobbyScene");
        GameManager.Instance.LoadNextScene(GameScene.TEMP_LOBBY, false);
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            //StartCoroutine(SpawnAsteroid());
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CheckEndOfGame();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey(PLAYER_LIVES))
        {
            CheckEndOfGame();
            return;
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (changedProps.ContainsKey(PLAYER_LOADED_LEVEL))
        {
            if (CheckAllPlayerLoadedLevel())
            {
                Hashtable props = new Hashtable
                    {
                        {CountdownTimer.CountdownStartTime, (float) PhotonNetwork.Time}
                    };
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            }
        }
    }

    #endregion

    #region coroutines

    private IEnumerator SpawnSheetMusic()
    {
        while (true)
        {
            /*
            int randomInt = 0;
            while (true)
            {
                yield return YieldInstructionCache.WaitForSeconds(5.0f);
                randomInt = Random.RandomRange(0, 3); 
            }
            */
            // TODO : 맵 마다 정해진 위치에서 악보 스폰 되는게?, 악보 스폰 텀은 얼마나?(아마 악보마다 랜덤하게 하는게 낫지 않을까?)
            // 맵 마다 컨셉으로 리스폰 위치의 갯수, 시간 다르게 해도 좋을 듯??
        }
    }

    // TODO : 구조 언제든지 바뀔 수 있음.
    private IEnumerator EndOfGame(string winner, int score)
    {
        float timer = 5.0f;

        while (timer > 0.0f)
        {
            InfoText.text = string.Format("{0}\n\n로비 복귀 {1} 초 전", temp, timer.ToString("n2"));

            yield return new WaitForEndOfFrame();

            timer -= Time.deltaTime;
        }

        PhotonNetwork.LocalPlayer.SetScore(0);
        PhotonNetwork.LocalPlayer.SetNumSheetMusic(0);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("TempLobbyScene");
    }

    #endregion
}
