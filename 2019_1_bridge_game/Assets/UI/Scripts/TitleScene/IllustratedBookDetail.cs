using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IllustratedBookDetail : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text nameText;

    public void SetBookDetail(Sprite img, string name)
    {
        this.image.sprite = img;
        this.nameText.text = name;
    }

}
