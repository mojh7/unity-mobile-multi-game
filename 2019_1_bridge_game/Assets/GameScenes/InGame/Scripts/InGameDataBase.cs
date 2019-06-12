using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UBZ.Item;

public class InGameDataBase : MonoBehaviourSingleton<InGameDataBase>
{
    #region variables
    [SerializeField] private InGameItemData[] inGameItemDataList;
    [SerializeField] private Dictionary<string, InGameItemData> inGameItemDataDictionary;
    [SerializeField] private Sprite[] emoticonSprites;
    #endregion

    #region get / set
    public InGameItemData GetInGameItemData(int index)
    {
        return inGameItemDataList[index];
    }
    public InGameItemData GetInGameItemData(string itemName)
    {
        return inGameItemDataDictionary[itemName];
    }
    public InGameItemData GetInGameItemData(UBZ.Item.InGameBuffType type)
    {
        return inGameItemDataList[(int)type];
    }
    public Sprite GetEmoticonSprite(UBZ.Owner.CharacterInfo.EmoticonType type)
    {
        return emoticonSprites[(int)type];
    }
    /// <summary>
    /// 버프, 디버프 랜덤
    /// </summary>
    /// <returns></returns>
    public UBZ.Item.InGameBuffType GetInGameItemType()
    {
        return (UBZ.Item.InGameBuffType)Random.Range(0, inGameItemDataList.Length);
    }
    #endregion

    #region unityFunc
    private void Awake()
    {
        inGameItemDataDictionary = new Dictionary<string, InGameItemData>();
        foreach(InGameItemData inGameItemData in inGameItemDataList)
        {
            inGameItemDataDictionary[inGameItemData.itemName] = inGameItemData;
        }
    }
    #endregion

    #region func
    #endregion
}
