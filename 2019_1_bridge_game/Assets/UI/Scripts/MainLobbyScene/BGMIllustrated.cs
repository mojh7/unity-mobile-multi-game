using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class BGMIllustrated : MonoBehaviour
{
    [SerializeField] private GameObject bgmllBook;
    [SerializeField] private Transform scrollRect;
    [SerializeField] private BGMDatabase bgm_database;
    [SerializeField] private UIBuying buyingpanel;
    [SerializeField] private Sprite pause;

    private List<GameObject> illustratedBook = new List<GameObject>();
    private AudioSource audioSource;
    private Slider slider;
    private Sprite current;

    private void Start()
    {
        Initialized();
    }
    private void Update()
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

    public void Initialized() {
        illustratedBook.Clear();
        //CharacterDatabase data = DatabaseManager.Instance.characterData;
        BGMDatabase data = bgm_database;
        int cnt = data.dataList.Count;

        for (int i = 0; i < cnt; i++)
        {
            GameObject tmpBGM = Instantiate(bgmllBook, scrollRect);
            BGMBook tmpIllustrateBook = tmpBGM.GetComponent<BGMBook>();

            string name = data.dataList[i].name;
            AudioClip bgm = data.dataList[i].bgm;

            tmpIllustrateBook.Init(bgm, name);
            tmpIllustrateBook.GetBuyingBtn().onClick.AddListener(() => AddListenBuying(name));
            Slider tmpslider = tmpIllustrateBook.GetSlider();
            audioSource = this.GetComponent<AudioSource>();
            tmpIllustrateBook.GetPlayBtn().onClick.AddListener(() => AddListenPlay(bgm, tmpslider));
            tmpIllustrateBook.GetPauseBtn().onClick.AddListener(()=> AddListenPause(bgm));
            tmpIllustrateBook.GetStopBtn().onClick.AddListener(() => AddListenStop());
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
    }
    private void AddListenPause(AudioClip bgm)
    {
        if(audioSource.isPlaying == true)
        {
            audioSource.Pause();
            AudioManager.Instance.ResumeMusic();
        }
    }


    private void AddListenStop()
    {
        if (audioSource.isPlaying == true)
        {

            audioSource.Stop();
            AudioManager.Instance.ResumeMusic();
        }
        slider.value = 0;
    }
    /*
    private int findObject(AudioClip bgm)
    {
        int cnt = illustratedBook.Count;
        for(int i = 0; i < cnt; i++)
        {
            if (bgm == illustratedBook[i].GetComponent<BGMBook>().GetBGM()) { 
                return i;
            }
        }
        return -1;
    }
    */
}
