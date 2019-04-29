﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterIllustrated : MonoBehaviour
{
    [SerializeField] private GameObject characterllBook;
    [SerializeField] private Transform scrollRect;
    [SerializeField] private CharacterBookDetail bookDetail;
    [SerializeField] private CharacterDatabase ch_database;
    [SerializeField] private UICloset closet;
    [SerializeField] private Button closet_btn;
    
    private List<GameObject> illustratedBook = new List<GameObject>();
    private int ch_id;

    private void Start()
    {
        // 나중에 UI Controller로 옮길 것 !
        Initialized();
        closet_btn.onClick.AddListener(() => AddListenSkinDetail());
    }

    // 시작 시, 한 번 캐릭터 데이터베이스 로드
    public void Initialized()
    {
        Debug.Log("CharacterBook : Initialized");
        illustratedBook.Clear();
        //CharacterDatabase data = DatabaseManager.Instance.characterData;
        CharacterDatabase data = ch_database;
        // 도감의 윗 줄 UI
        int cnt = data.dataList.Count;
        for (int i = 0; i < cnt; i++)
        {
            GameObject tmpCharacter = Instantiate(characterllBook, scrollRect);
            CharacterBook tmpIllustrateBook = tmpCharacter.GetComponent<CharacterBook>();

            int id = data.dataList[i].id;
            string name = data.dataList[i].name;
            Sprite sprite = data.dataList[i].sprite;
            int gen = data.dataList[i].genius;
            int social = data.dataList[i].sociability;
            int health = data.dataList[i].health;
            string playverText = data.dataList[i].playverText;
            string characteristic = data.dataList[i].characteristic;
            /*

           if (BackendController.Instance.GetBgmDIct().ContainsKey(name))
           {
               //해금되지 않은 캐릭터
               if(BackendController.Instance.GetBgmDIct().ContainsValue(0))
               {
                   tmpIllustrateBook.GetBuyingBtn().onClick.AddListener(() => AddListenBuying(name));
               }
           }


            */
            tmpIllustrateBook.Init(sprite, name);
            tmpIllustrateBook.GetButton().onClick.AddListener(() => AddListenCharacterDetail(sprite, name, gen, social, health, id, playverText, characteristic));

            illustratedBook.Add(tmpCharacter);
        }

        characterllBook.SetActive(false);
        illustratedBook[0].GetComponent<UnityEngine.UI.Button>().onClick.Invoke();

    }

    private void AddListenCharacterDetail(Sprite img, string name, int gen, int social, int health, int id, string playverText, string characteristic)
    {
        bookDetail.SetBookDetail(img, name, gen, social, health, id, playverText, characteristic);
    }
    private void AddListenSkinDetail()
    {
        ch_id = bookDetail.GetId();
        Debug.Log("ch_id ? " + ch_id);
        closet.SetId(ch_id);
    }
    public int GetId()
    {
        return ch_id;
    }

}
