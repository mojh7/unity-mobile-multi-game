using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템이라 부를만 한게 다양하고, 그 다른 것이 Item이라는 이름으로 클래스를 따로 만들거나 상속 받아서
// 구조 짤 수도 있어 보여서, 일단 인 게임 아이템들은 InGameItemData class 따로 만듬
public abstract class ItemData : ScriptableObject
{
    [SerializeField] protected string itemName;
    [Tooltip("화면에 표시할 설명문. 툴팁 개념")]
    [SerializeField] protected string description;
}
