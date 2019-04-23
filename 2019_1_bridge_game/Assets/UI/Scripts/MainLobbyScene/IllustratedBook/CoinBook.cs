using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinBook : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Image image;
    [SerializeField] private Button buyingbtn;

    public void Init(Sprite sprite, string name)
    {
        image.sprite = sprite;
        nameText.text = name;
    }

    public Button GetButton() { return buyingbtn; }
}
