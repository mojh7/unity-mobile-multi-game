using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBookDetail : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text nameText;
    [SerializeField] private Slider gen;
    [SerializeField] private Slider social;
    [SerializeField] private Slider health;

    public void SetBookDetail(Sprite img, string name, int gen, int social, int health)
    {
        this.image.sprite = img;
        this.nameText.text = name;
        this.gen.value = gen;
        this.social.value = social;
        this.health.value = health;
        Debug.Log("value : "+ gen + social + health);
    }

}