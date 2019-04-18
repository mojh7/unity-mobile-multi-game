
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMBook : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Button buyingBtn;
    [SerializeField] private Button play;
    [SerializeField] private Button pause;
    [SerializeField] private Button stop;
    [SerializeField] private Slider slider;

    private AudioClip bgm;

    public void Init(AudioClip bgm, string name)
    {
        this.bgm = bgm;
        nameText.text = name;
    }

    public Button GetBuyingBtn() { return buyingBtn; }
    public Button GetPlayBtn() { return play; }
    public Button GetPauseBtn() { return pause; }
    public Button GetStopBtn() { return stop; }
    public Slider GetSlider() { return slider; }
}
