using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinBookIllustrate : MonoBehaviour
{
    [SerializeField] private GameObject skinllBook;
    [SerializeField] private Transform scrollRect;
    [SerializeField] private SkinDatabase sk_database;
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
        SkinDatabase data = sk_database;
        int cnt = data.dataList.Count;

        for (int i = 0; i < cnt; i++)
        {
            GameObject tmpSkin = Instantiate(skinllBook, scrollRect);
            SkinBook tmpIllustrateBook = tmpSkin.GetComponent<SkinBook>();

            string name = data.dataList[i].name;
            Sprite sprite = data.dataList[i].sprite;

            tmpIllustrateBook.Init(sprite, name);
            tmpIllustrateBook.GetButton().onClick.AddListener(() => AddListenBuying(sprite, name));
            illustratedBook.Add(tmpSkin);
        }
       skinllBook.SetActive(false);
       //illustratedBook[0].GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
    }

    private void AddListenBuying(Sprite img, string name)
    {
        Debug.Log("show buying panel");
        buyingpanel.setBuyingpanel(img, name);
        buyingpanel.OnShow();
    }

}
