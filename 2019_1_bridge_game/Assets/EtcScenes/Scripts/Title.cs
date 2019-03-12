using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    #region variables
    [SerializeField]
    private Transform titleTransform;
    private Vector3 titleScale;

    // --- 임시 닉네임 설정 위치
    [SerializeField] private Text nicknameText;
    #endregion

    #region unityFunc
    private void Start()
    {
        InitTitle();
    }
    #endregion

    #region func
    private void InitTitle()
    {
        AudioManager.Instance.PlayMusic(0);
        titleScale = titleTransform.localScale;
        StartCoroutine(RepeatTitleScaleBiggerAndSmaller());
    }

    public void LoadIngame()
    {
        AudioManager.Instance.StopMusic();

        // TO DO 
        // --- 특수문자, 길이, 공백 체크
        // --- 경고 팝업 또는 생성 확인 팝업
        GameDataManager.Instance.userData.SetNickname(nicknameText.text);

        GameManager.Instance.LoadNextScene(GameScene.IN_GAME, true);
    }
    #endregion

    #region coroutine
    IEnumerator RepeatTitleScaleBiggerAndSmaller()
    {
        float time = 0;
        while (true)
        {
            titleTransform.localScale = titleScale * (1f + 0.07f * Mathf.Sin(time));
            time += Time.fixedDeltaTime * 3f;
            yield return YieldInstructionCache.WaitForSeconds(Time.fixedDeltaTime);
        }
    }
    #endregion



}
