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

    private void RefreshTopLayout()
    {
        Debug.Log("Refresh Top Layout : User Data");
        nicknameText.text   = BackendController.Instance.GetUserNickName();
        levelText.text      = BackendController.Instance.GetUserLevel();
        coinText.text       = BackendController.Instance.GetUserCoinData();
    }
}