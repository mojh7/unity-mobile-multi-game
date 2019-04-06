using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendData : MonoBehaviour
{
    [SerializeField] private Text nickText;
    [SerializeField] private Text levelText;
    [SerializeField] private Image sprite;
    private string indate;

    public void SetFriendImage(string nick, string lv, string indate, Sprite sp = null)
    {
        nickText.text = nick;
        levelText.text = lv;
        sprite.sprite = sp;
        this.indate = indate;
        this.gameObject.SetActive(true);
    }

    public void AcceptFriend()
    {
        if (indate.Equals(null)) Debug.Log("indate 값이 null입니다. 뒤끝을 확인해주세요.");

        BackendController.Instance.AcceptFriendRequest(indate);
        this.gameObject.SetActive(false);
    }

    public void RejectFriend()
    {
        if (indate.Equals(null)) Debug.Log("indate 값이 null입니다. 뒤끝을 확인해주세요.");

        BackendController.Instance.RejectFriendRequest(indate);
        this.gameObject.SetActive(false);
    }

    public void BreakFriend()
    {
        if (indate.Equals(null)) Debug.Log("indate 값이 null입니다. 뒤끝을 확인해주세요.");

        BackendController.Instance.BreakFriend(indate);
        this.gameObject.SetActive(false);
    }

    public void RevokeFriend()
    {
        if (indate.Equals(null)) Debug.Log("indate 값이 null입니다. 뒤끝을 확인해주세요.");

        BackendController.Instance.RevokeFriendRequest(indate);
        this.gameObject.SetActive(false);
    }
}
