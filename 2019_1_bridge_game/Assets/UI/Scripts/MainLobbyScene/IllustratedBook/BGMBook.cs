
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

    private int id;
    public void Init(int id, string name)
    {
        this.id = id;
        nameText.text = name;
    }

    public Button GetBuyingBtn() { return buyingBtn; }
    public Button GetPlayBtn() { return play; }
    public Button GetPauseBtn() { return pause; }
    public Button GetStopBtn() { return stop; }
    public Slider GetSlider() { return slider; }

}
