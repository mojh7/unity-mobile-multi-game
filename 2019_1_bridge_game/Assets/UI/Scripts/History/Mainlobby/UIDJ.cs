using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class UIDJ : UIControl
{
    #region variable
    [SerializeField] private GameObject bgmllBook;
    [SerializeField] private Transform scrollRect;
    [SerializeField] private BGMDatabase bgm_database;
    [SerializeField] private UIBuying buyingpanel;
    [SerializeField] private UIChoose choosepanel;

    private List<GameObject> illustratedBook = new List<GameObject>();
    private AudioSource audioSource;
    private Slider slider;
    #endregion variable

    private void Start()
    {
        Initialized();
    }
    private void Update()
    {
        //슬라이더 움직이기
        /*
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
        }*/
        moveSlider();
    }

    public void Initialized()
    {
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
            Slider tmpslider = tmpIllustrateBook.GetSlider();
            audioSource = this.GetComponent<AudioSource>();

            tmpIllustrateBook.GetPlayBtn().onClick.AddListener(() => AddListenPlay(bgm, tmpslider));
            tmpIllustrateBook.GetPauseBtn().onClick.AddListener(() => AddListenPause(bgm));
            tmpIllustrateBook.GetStopBtn().onClick.AddListener(() => AddListenStop(tmpslider));

            /*
           
            if (BackendController.Instance.GetBgmDIct().ContainsKey(name))
            {
                //구매되지 않은 bgm
                if(BackendController.Instance.GetBgmDIct().ContainsValue(0))
                {
                    tmpIllustrateBook.GetBuyingBtn().onClick.AddListener(() => AddListenBuying(name));
                }
                //구매완료된 bgm
                else
                {
                    tmpIllustrateBook.GetBuyingBtn().onClick.AddListener(() => AddListenSwith());
                }
            }


             */
            //tmpIllustrateBook.GetBuyingBtn().onClick.AddListener(() => AddListenBuying(name));
            tmpIllustrateBook.GetBuyingBtn().onClick.AddListener(() => AddListenSwith(id,name));
            illustratedBook.Add(tmpBGM);
        }
        bgmllBook.SetActive(false);
    }

    private void AddListenBuying(string name)
    {
        Debug.Log("show buying panel");
        buyingpanel.setBuyingpanel(name);
        buyingpanel.OnShow();
    }


    private void AddListenPlay(AudioClip bgm, Slider slider)
    {

        if (audioSource.clip == null || audioSource.clip != bgm || audioSource.isPlaying == false)
        {
            AudioManager.Instance.PauseMusic();
            audioSource.clip = bgm;
            this.slider = slider;
            this.slider.maxValue = bgm.length;

            audioSource.Play();
        }

        else if (audioSource.clip == bgm && audioSource.isPlaying == false)
        {
            audioSource.UnPause();
        }
        /*
        else if(audioSource.clip == bgm && audioSource.isPlaying == true)
        {
            audioSource.UnPause();
            audioSource.clip = bgm;
            audioSource.Play();
        }*/
    }
    private void AddListenPause(AudioClip bgm)
    {
        if (audioSource.isPlaying == true)
        {
            audioSource.Pause();
            AudioManager.Instance.ResumeMusic();
        }
    }


    private void AddListenStop(Slider slider)
    {
        if (audioSource.isPlaying == true)
        {
            audioSource.Stop();
            AudioManager.Instance.ResumeMusic();
        }
        slider.value = 0;
    }

    private void AddListenSwith(int id, string name)
    {
        choosepanel.SetItem(name);
        UIManager.Instance.ShowNew(choosepanel);
        if(audioSource.clip != null)
        {
            audioSource.Stop();
            slider.value = 0;
        }
        audioSource.clip = null;

        AudioManager.Instance.PlayMusic(id);
    }
    
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

}
