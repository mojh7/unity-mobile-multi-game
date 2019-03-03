using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    #region variables
    [SerializeField]
    private Transform titleTransform;
    private Vector3 titleScale;
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
