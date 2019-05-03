using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//캐릭터 정보 창
public class CharacterBookDetail : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Image image;
    [SerializeField] private Text nameText;
    [SerializeField] private Slider gen;
    [SerializeField] private Slider social;
    [SerializeField] private Slider health;
    [SerializeField] private Text playverText;
    [SerializeField] private Text characteristic;

    public void SetBookDetail(Sprite img, string name, int gen, int social, int health, int id,string playverText, string characteristic)
    {
        this.id = id;
        this.image.sprite = img;
        this.nameText.text = name;
        this.gen.value = gen;
        this.social.value = social;
        this.health.value = health;
        this.playverText.text = playverText;
        this.characteristic.text = characteristic;
    }

    public int GetId()
    {
        return id;
    }
}