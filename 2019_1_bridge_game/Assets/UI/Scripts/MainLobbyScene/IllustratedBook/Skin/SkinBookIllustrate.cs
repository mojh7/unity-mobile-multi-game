using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//스킨 도감
public class SkinBookIllustrate : MonoBehaviour
{
    #region variable
    [SerializeField] private GameObject skinllBook;
    [SerializeField] private Transform scrollRect;
    [SerializeField] private SkinDatabase sk_database;
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
    //스킨 도감 초기화 - 스크롤 패널
    private void Initialized()
    {
        illustratedBook.Clear();
        //CharacterDatabase data = DatabaseManager.Instance.characterData;
        SkinDatabase data = sk_database;
        int cnt = data.dataList.Count;

        for (int i = 0; i < cnt; i++)
        {
            GameObject tmpSkin = Instantiate(skinllBook, scrollRect);
            SkinBook tmpIllustrateBook = tmpSkin.GetComponent<SkinBook>();

            string name = data.dataList[i].name;
            Sprite sprite = data.dataList[i].sprite;

            tmpIllustrateBook.Init(sprite, name);
            tmpIllustrateBook.GetButton().onClick.AddListener(() => AddListenBuying(name));
            illustratedBook.Add(tmpSkin);
        }
       skinllBook.SetActive(false);
       //illustratedBook[0].GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
    }
    //구매 버튼 활성화
    private void AddListenBuying(string name)
    {
        Debug.Log("show buying panel");
        buyingpanel.setBuyingpanel(name);
        buyingpanel.OnShow();
    }
    #endregion
}
