using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emoticon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer emoticon;
    [SerializeField] private SpriteRenderer speechBubble;

    private void Awake()
    {
        SetActiveSprites(false);
    }

    public void SetActiveSprites(bool active)
    {
        emoticon.enabled = active;
        speechBubble.enabled = active;
    }

    public void ShowEmoticon(UBZ.Owner.CharacterInfo.EmoticonType type)
    {
        SetActiveSprites(true);

        emoticon.sprite = InGameManager.Instance.GetEmoticonSprite(type);
        UtilityClass.Invoke(this, ()=> { SetActiveSprites(false); }, 2f);
    }
}
