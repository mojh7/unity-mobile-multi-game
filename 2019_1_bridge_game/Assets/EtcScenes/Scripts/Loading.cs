using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    #region variables
    private static readonly string[] LOADING_TEXT = new string[]{"로딩중", "로딩중.", "로딩중..", "로딩중..."};

    [SerializeField]
    private float minTime = 0f; //로딩씬이 유지되는 최소 시간
    [SerializeField]
    private Text loadingTxt;
    [SerializeField]
    private Image image = null;
    [SerializeField]
    private Text tipTxt = null; //팁 텍스트
    [SerializeField]
    private Slider loadingSliderbar = null; //하단 슬라이더바
    //[SerializeField]
    //private Transform wheel = null;//중앙 회전 이미지
    [SerializeField]
    private string[] tips = null;//팁 텍스트 모음
    [SerializeField]
    private Sprite[] loadingSprites;
    [SerializeField]
    private RuntimeAnimatorController[] anim;
    private int selectChar;
    private bool isLoad = false; //중복 실행 방지
    private float timer = 0; //시간 측정
    private float totalGage;
    //private Vector3 wheeleuler;//중앙 회전 이미지 오일러 각도측정용
    AsyncOperation async;
    #endregion

    #region get / set
    #endregion

    #region unityFunc
    void Start()
    {
        InitLoading();
    }
    private void Update()
    {
        if (async == null)
            return;
        timer += Time.deltaTime;
    }
    #endregion

    #region func
    private void InitLoading()
    {
        //RectTransform imageTransform = image.GetComponent<RectTransform>();
        //RectTransform tipTransform = tipText.GetComponent<RectTransform>();
        //RectTransform sliderbarTransform = loadingSliderbar.GetComponent<RectTransform>();
        //imageTransform.sizeDelta = new Vector2(Screen.width, Screen.height * 0.3f);
        //imageTransform.anchoredPosition = new Vector3(imageTransform.localPosition.x, Screen.height * 0.55f, imageTransform.localPosition.z);
        //tipTransform.anchoredPosition = new Vector3(tipTransform.localPosition.x, Screen.height * 0.2f, tipTransform.localPosition.z);
        //sliderbarTransform.sizeDelta = new Vector2(sliderbarTransform.sizeDelta.x, Screen.height * 0.05f);
        //tipText.fontSize = (int)(Screen.height * 0.1f);

        if (0 < loadingSprites.Length)
            image.sprite = loadingSprites[Random.Range(0, loadingSprites.Length)];
        //image.GetComponent<Animator>().runtimeAnimatorController = anim[selectChar];
        if (tips.Length <= 0)
            return;
        int tipIndex = Random.Range(0, tips.Length); //배열내에서 무작위로 인덱스를 얻는다.
        /*StringBuilder sb = new StringBuilder("팁 : ");
        sb.Append(Tips[loc]); //배열 내 무작위 요소를 출력한다.
        Tip.text = sb.ToString();*/
        tipTxt.text = tips[tipIndex];
        totalGage = minTime;
        Time.timeScale = 1;

        StartCoroutine(ShowLoadingText());
        StartCoroutine(LoadingNextScene());
    }
    #endregion

    #region coroutine

    IEnumerator ShowLoadingText()
    {
        int index = 0;
        while(true)
        {
            loadingTxt.text = LOADING_TEXT[index];
            index = (index + 1) % LOADING_TEXT.Length;
            yield return YieldInstructionCache.WaitForSeconds(0.05f * Random.Range(1, 6));
        }
    }

    // 비동기 scene 로딩처리와 진행바 progress bar
    IEnumerator LoadingNextScene()
    {
        if (isLoad == false)
        {
            isLoad = true;
            async = SceneManager.LoadSceneAsync(GameManager.Instance.GetNextScene());
            async.allowSceneActivation = false; //다음 씬의 준비가 완료되더라도 바로 로딩되는걸 막는다.
            while (!async.isDone)
            {
                if (async.progress == 0.9f && timer >= minTime)
                {
                    loadingSliderbar.value = 1;
                    async.allowSceneActivation = true;
                }
                else
                {
                    loadingSliderbar.value = (async.progress + timer) / totalGage;
                }
                yield return null;
            }
        }
    }
    #endregion
}