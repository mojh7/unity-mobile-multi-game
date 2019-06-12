using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emoticon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer emoticonSprite;

    private void Awake()
    {
        emoticonSprite.enabled = false;
    }

    public void ShowEmoticon(UBZ.Owner.CharacterInfo.EmoticonType type)
    {
        emoticonSprite.enabled = true;
        emoticonSprite.sprite = InGameDataBase.Instance.GetEmoticonSprite(type);
        UtilityClass.Invoke(this, ()=> { emoticonSprite.enabled = false; }, 2f);
    }
}
