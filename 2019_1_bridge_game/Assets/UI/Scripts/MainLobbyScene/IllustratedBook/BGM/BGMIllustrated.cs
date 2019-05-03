using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//BGM 상점 도감
[RequireComponent(typeof(AudioSource))]
public class BGMIllustrated : MonoBehaviour
{
    #region variable
    [SerializeField] private GameObject bgmllBook;
    [SerializeField] private Transform scrollRect;
    [SerializeField] private BGMDatabase bgm_database;
    [SerializeField] private UIBuying buyingpanel;

    private List<GameObject> illustratedBook = new List<GameObject>();
    private AudioSource audioSource;
    private Slider slider;
    #endregion

    #region Unityfuc
    private void Start()
    {
        Initialized();
    }
    private void Update()
    {
        //음악 슬라이더 이동
        moveSlider();
    }
    #endregion

    #region fuc
    //BGM 상점 초기화
    public void Initialized() {
        illustratedBook.Clear();
        //CharacterDatabase data = DatabaseManager.Instance.characterData;
        BGMDatabase data = bgm_database;
        int cnt = data.dataList.Count;

        for (int i = 0; i < cnt; i++)
        {
            GameObject tmpBGM = Instantiate(bgmllBook, scrollRect);
            BGMBook tmpIllustrateBook = tmpBGM.GetComponent<BGMBook>();

            int id = data.dataList[i].id;
            string name = data.dataList[i].name;
            AudioClip bgm = data.dataList[i].bgm;

            tmpIllustrateBook.Init(id, name);
            tmpIllustrateBook.GetBuyingBtn().onClick.AddListener(() => AddListenBuying(name));
            Slider tmpslider = tmpIllustrateBook.GetSlider();
            audioSource = this.GetComponent<AudioSource>();
            tmpIllustrateBook.GetPlayBtn().onClick.AddListener(() => AddListenPlay(bgm, tmpslider));
            tmpIllustrateBook.GetPauseBtn().onClick.AddListener(()=> AddListenPause(bgm));
            tmpIllustrateBook.GetStopBtn().onClick.AddListener(() => AddListenStop(tmpslider));
            illustratedBook.Add(tmpBGM);
        }
        bgmllBook.SetActive(false);
    }

    //bgm 이름 클릭시 - 구매상태X 일때
    private void AddListenBuying(string name)
    {
        if (audioSource.isPlaying == true)
        {
            audioSource.Stop();
        }
        Debug.Log("show buying panel");
        buyingpanel.setBuyingpanel(name);
        buyingpanel.OnShow();
    }

    //play 버튼 클릭시
    private void AddListenPlay(AudioClip bgm, Slider slider)
    {
        //플레이 중인 음악이 없을 때
        if (audioSource.clip == null || audioSource.clip != bgm || audioSource.isPlaying == false)
        {
            AudioManager.Instance.PauseMusic();
            audioSource.clip = bgm;
            this.slider = slider;
            this.slider.maxValue = bgm.length;

            audioSource.Play();
        }

        //음악이 플레이 중 일 때
        else if (audioSource.clip == bgm && audioSource.isPlaying == false)
        {
            audioSource.UnPause();
        }
    }

    //pause 버튼 클릭시
    private void AddListenPause(AudioClip bgm)
    {
        if (audioSource.isPlaying == true)
        {
            audioSource.Pause();
            AudioManager.Instance.ResumeMusic();
        }
    }

    //stop 버튼 클릭시
    private void AddListenStop(Slider slider)
    {
        //플레이 중
        if (audioSource.isPlaying == true)
        {
            audioSource.Stop();
            AudioManager.Instance.ResumeMusic();
        }
        slider.value = 0;
    }

    //bgm-슬라이더 표시
    private void moveSlider()
    {
        if (slider != null)
        {
            if (audioSource.isPlaying == true)
            {
                slider.value += Time.deltaTime;
            }
            else
            {
                if (slider.maxValue == slider.value)
                    slider.value = 0;
            }
        }
    }
    #endregion
}
