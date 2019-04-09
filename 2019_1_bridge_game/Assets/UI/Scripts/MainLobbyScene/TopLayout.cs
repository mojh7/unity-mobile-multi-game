using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopLayout : MonoBehaviour
{
    [SerializeField] private Text nicknameText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text coinText;


    private void OnEnable()
    {
        RefreshTopLayout();
    }

    // TODO
    // 아이템 구매 or 유저의 정보가 변할 때 꼭 호출.
    public void RefreshTopLayout()
    {
        Debug.Log("Refresh Top Layout : User Data");
        nicknameText.text   = BackendController.Instance.GetUserNickName();
        levelText.text      = BackendController.Instance.GetUserLevel();
        coinText.text       = BackendController.Instance.GetUserCoinData();
    }
}