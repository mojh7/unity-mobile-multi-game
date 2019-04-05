﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICloset : UIControl
{
    [SerializeField] private GameObject skinllBook;
    [SerializeField] private Transform scrollRect;
    [SerializeField] private SkinBookDetail bookDetail;
    [SerializeField] private SkinDatabase ch_database;
    [SerializeField] private CharacterIllustrated character;
    [SerializeField] private Button exit;

    private List<GameObject> illustratedBook = new List<GameObject>();
    private int ch_id;
    private GameObject target;

    /*
    private void Start()
    {
        LoadSkin();
    }
    */
    private void OnEnable()
    {
        Debug.Log("destroy");
        DestroySkin();
        LoadSkin();
    }
    private void OnDisable()
    {
        //Debug.Log("destroy");
      //  DestroySkin();
    }
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
                tmpIllustrateBook.GetButton().onClick.AddListener(() => AddListenCharacterDetail(sprite, name,text));

                illustratedBook.Add(tmpSkin);
                
            }
        }

        Destroy(skinllBook);
       //skinllBook.SetActive(false);
        skinllBook = illustratedBook[0];
        illustratedBook[0].GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
        
    }

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

    private void AddListenCharacterDetail(Sprite img, string name, string text)
    {
        bookDetail.SetBookDetail(img, name, text);
    }

    public int GetId()
    {
        return ch_id;
    }

    public void SetId(int id)
    {
        ch_id = id;
    }

}