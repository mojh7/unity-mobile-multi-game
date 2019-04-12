using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinIllustrated : MonoBehaviour
{
    [SerializeField] private GameObject coinllBook;
    [SerializeField] private Transform scrollRect;
    [SerializeField] private CoinDatabase co_database;
    [SerializeField] private UIBuying buyingpanel;

    private List<GameObject> illustratedBook = new List<GameObject>();

    private void Start()
    {
        Initialized();
    }

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
            tmpIllustrateBook.GetButton().onClick.AddListener(() => AddListenBuying(sprite, name));
            illustratedBook.Add(tmpCoin);
        }
        coinllBook.SetActive(false);
        //illustratedBook[0].GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
    }

    private void AddListenBuying(Sprite img, string name)
    {
        Debug.Log("show buying panel");
        buyingpanel.setBuyingpanel(img, name);
        buyingpanel.OnShow();
    }

}
