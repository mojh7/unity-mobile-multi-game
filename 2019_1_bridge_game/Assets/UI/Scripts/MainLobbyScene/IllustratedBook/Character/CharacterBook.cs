using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//캐릭터 도감 버튼
public class CharacterBook : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text nameText;
    [SerializeField] private Button detailBtn;


    public void Init(Sprite sprite, string name)
    {
        image.sprite = sprite;
        nameText.text = name;
    }

    public Button GetButton() { return detailBtn; }
}