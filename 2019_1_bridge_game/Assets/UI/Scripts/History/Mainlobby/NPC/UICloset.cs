using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//스킨 도감(옷장)
public class UICloset : UIControl
{
    #region variable
    [SerializeField] private GameObject skinllBook;
    [SerializeField] private Transform scrollRect;
    [SerializeField] private SkinBookDetail bookDetail;
    [SerializeField] private SkinDatabase ch_database;
    [SerializeField] private UIBuying buyingpanel;

    private List<GameObject> illustratedBook = new List<GameObject>();
    private int ch_id;
    #endregion

    #region Unityfuc
    private void OnEnable()
    {
        //Debug.Log("destroy");
        //기존 옷장 파괴 후 새 옷장 로드
        DestroySkin();
        LoadSkin();
    }
    private void Update()
    {
        string name = bookDetail.GetnameText();
        //스킨 별로 구매 창 업데이트
        bookDetail.GetButton().onClick.AddListener(() => AddListenBuying(name));
    }
    #endregion

    #region fuc
    //캐릭터별로 스킨 로드
    public void LoadSkin()
    {
        Debug.Log("LoadSkin " + ch_id);
        illustratedBook.Clear();
        SkinDatabase data = ch_database;

        // 도감의 윗 줄 UI
        int cnt = data.dataList.Count;
        for (int i = 0; i < cnt; i++)
        {
            if (data.dataList[i].id == ch_id)
            {
                GameObject tmpSkin = Instantiate(skinllBook, scrollRect);
                SkinBook tmpIllustrateBook = tmpSkin.GetComponent<SkinBook>();

                int id = data.dataList[i].id;
                Debug.Log("LoadSkin " + id);
                string name = data.dataList[i].name;
                Debug.Log("LoadSkin " + name);
                Sprite sprite = data.dataList[i].sprite;
                string text = data.dataList[i].characteristic;
                tmpIllustrateBook.Init(sprite, name);
                tmpIllustrateBook.GetButton().onClick.AddListener(() => AddListenSkinDetail(sprite, name,text));
                illustratedBook.Add(tmpSkin);
                
            }
        }

        Destroy(skinllBook);
        skinllBook = illustratedBook[0];
        illustratedBook[0].GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
        
    }

   //이전 패널 삭제
   private void DestroySkin()
    {

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Skin");
        Debug.Log("objects.Length " + objects.Length);
        for (int i = 1; i < objects.Length; i++)
        {
            Debug.Log("destroy : " + objects[i].name);
            Destroy(objects[i]);
        }
        skinllBook.SetActive(true);

    }

    //스킨 창 정보 업데이트
    private void AddListenSkinDetail(Sprite img, string name, string text)
    {
        bookDetail.SetBookDetail(img, name, text);

    }
    //구매창 업데이트 및 활성화
    private void AddListenBuying( string name)
    {
        buyingpanel.setBuyingpanel(name);
        buyingpanel.OnShow();
    }

    public int GetId()
    {
        return ch_id;
    }

    public void SetId(int id)
    {
        ch_id = id;
    }
    #endregion
}
