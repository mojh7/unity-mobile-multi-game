using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendController : MonoBehaviour
{
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private Toggle listToggle;
    [SerializeField] private Toggle requestToggle;
    [SerializeField] private InputField friendNickInput;
    [SerializeField] private Button friendAddBtn;
    [SerializeField] private GameObject list;
    [SerializeField] private GameObject request;
    [SerializeField] private GameObject friendObj;
    [SerializeField] private GameObject requestObj;
    [SerializeField] private GameObject sentObj;

    private const int maxFriend = 15;

    private bool isInitialized = false;
    
    // TODO
    // 최적화.
    // 모바일로 했을 시 느릴 수 있음.
    public void Initialized()
    {
        if (!isInitialized)
        {
            isInitialized = true;

            CreateFriendList();
        }
    }

    // 초기 게임 실행시 목록 생성
    private void CreateFriendList()
    {
        Transform friendParent = friendObj.transform.parent;
        Transform requestParent = requestObj.transform.parent;
        Transform sentParent = sentObj.transform.parent;
        for (int i = 0; i < maxFriend; i++)
        {
            Instantiate(friendObj, friendParent);
            Instantiate(requestObj, requestParent);
            Instantiate(sentObj, sentParent);
        }
    }

    public void AddFriend()
    {
        string nick = friendNickInput.text;
        friendAddBtn.onClick.AddListener(() => BackendController.Instance.AddFriendUpdateBackend(nick));
        friendNickInput.text = "";

        CreateRequestData();
    }

    public void InputFriendData()
    {
        ListActiveFalseAll(friendObj);
        var list = BackendController.Instance.GetFriendList();

        int count = list.Item1;
        string[] nick, Indate, timeAt;
        nick    = list.Item2;
        Indate  = list.Item3;
        timeAt  = list.Item4;

        Transform parent = friendObj.transform.parent;
        for (int i = 0; i < count; i++)
        {
            FriendData fd = parent.GetChild(i).GetComponent<FriendData>();
            fd.SetFriendImage(nick[i], "temp", Indate[i], null);
        }
    }

    public void CreateRequestData()
    {
        ListActiveFalseAll(requestObj);
        ListActiveFalseAll(sentObj);

        var requList = BackendController.Instance.GetReceivedFriendRequestList();
        var snetList = BackendController.Instance.GetSentFriendRequestList();

        Transform requParent = requestObj.transform.parent;
        for (int i = 0; i < requList.Item1; i++)
        {
            FriendData reFd = requParent.GetChild(i).GetComponent<FriendData>();
            reFd.SetFriendImage(requList.Item2[i], "temp", requList.Item3[i], null);
        }

        Transform sentParent = sentObj.transform.parent;
        for (int i = 0; i < snetList.Item1; i++)
        {
            FriendData seFd = sentParent.GetChild(i).GetComponent<FriendData>();
            seFd.SetFriendImage(snetList.Item2[i], "temp", snetList.Item3[i], null);
        }
    }

    private void ListActiveFalseAll(GameObject obj)
    {
        Transform parent = obj.transform.parent;
        for (int i = 0; i < parent.childCount; i++)
        {
            parent.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void ActiveFriendList()
    {
        if (listToggle.isOn)
        {
            list.SetActive(true);
            request.SetActive(false);
            InputFriendData();
        }
    }

    public void ActiveFriendRequest()
    {
        if (requestToggle.isOn)
        {
            list.SetActive(false);
            request.SetActive(true);
            CreateRequestData();
        }
    }
}
