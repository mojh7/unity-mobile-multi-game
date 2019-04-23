using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinBookDetail : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text nameText;
    [SerializeField] private Text characteristicText;
    [SerializeField] private Button BuyingBtn;

    public void SetBookDetail(Sprite img, string name, string text)
    {
        image.sprite = img;
        nameText.text = name;
        characteristicText.text = text;
    }

    public Sprite GetImage()
    {
        return image.sprite;
    }

    public string GetnameText()
    {
        return nameText.text;
    }
    public Button GetButton() { return BuyingBtn; }
}
