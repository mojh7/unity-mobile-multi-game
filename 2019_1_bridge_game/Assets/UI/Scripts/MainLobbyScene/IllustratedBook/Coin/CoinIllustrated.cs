using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//코인 구매창
public class CoinIllustrated : MonoBehaviour
{
    #region variable
    [SerializeField] private GameObject coinllBook;
    [SerializeField] private Transform scrollRect;
    [SerializeField] private CoinDatabase co_database;
    [SerializeField] private UIBuying buyingpanel;

    private List<GameObject> illustratedBook = new List<GameObject>();
    #endregion

    #region Unityfuc
    private void Start()
    {
        Initialized();
    }
    #endregion

    #region fuc
    //구매 패널 초기화
    private void Initialized()
    {
        illustratedBook.Clear();
        //CharacterDatabase data = DatabaseManager.Instance.characterData;
        CoinDatabase data = co_database;
        int cnt = data.dataList.Count;

        for (int i = 0; i < cnt; i++)
        {
            GameObject tmpCoin = Instantiate(coinllBook, scrollRect);
            CoinBook tmpIllustrateBook = tmpCoin.GetComponent<CoinBook>();

            string name = data.dataList[i].name;
            Sprite sprite = data.dataList[i].sprite;

            tmpIllustrateBook.Init(sprite, name);
            tmpIllustrateBook.GetButton().onClick.AddListener(() => AddListenBuying(name));
            illustratedBook.Add(tmpCoin);
        }
        coinllBook.SetActive(false);
        //illustratedBook[0].GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
    }
    //구매창 띄우기
    private void AddListenBuying( string name)
    {
        Debug.Log("show buying panel");
        buyingpanel.setBuyingpanel(name);
        buyingpanel.OnShow();
    }
    #endregion
}
