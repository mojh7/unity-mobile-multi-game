using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICloset : UIControl
{
    [SerializeField] private GameObject characterllBook;
    [SerializeField] private Transform scrollRect;
    [SerializeField] private IllustratedBookDetail bookDetail;
    [SerializeField] private CharacterDatabase ch_database;

    private List<GameObject> illustratedBook = new List<GameObject>();

    private void Start()
    {
        // 나중에 UI Controller로 옮길 것 !
        Initialized();
    }

    // 시작 시, 한 번 캐릭터 데이터베이스 로드
    public void Initialized()
    {
        Debug.Log("CharacterBook : Initialized");
        illustratedBook.Clear();
        CharacterDatabase data = ch_database;

        // 도감의 윗 줄 UI
        int cnt = data.dataList.Count;
        for (int i = 0; i < cnt; i++)
        {
            GameObject tmpCharacter = Instantiate(characterllBook, scrollRect);
            IllustratedBook tmpIllustrateBook = tmpCharacter.GetComponent<IllustratedBook>();

            int id = data.dataList[i].id;
            string name = data.dataList[i].name;
            Sprite sprite = data.dataList[i].sprite;

            tmpIllustrateBook.Init(sprite, name);
            tmpIllustrateBook.GetButton().onClick.AddListener(() => AddListenCharacterDetail(sprite, name));

            illustratedBook.Add(tmpCharacter);
        }

        characterllBook.SetActive(false);
        illustratedBook[0].GetComponent<UnityEngine.UI.Button>().onClick.Invoke();

    }

    private void AddListenCharacterDetail(Sprite img, string name)
    {
        bookDetail.SetBookDetail(img, name);
    }
}
