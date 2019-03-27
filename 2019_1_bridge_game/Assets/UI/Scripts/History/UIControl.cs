using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    // Show, Hide 버튼에서 사용 시
    // UIManager의 HideAndShowPreview로 사용
    // 만약 문제가 있다면 변형 가능 

    public virtual void OnShow()
    {
        if (transform.parent.GetComponent<Animator>() != null)
        {
            transform.parent.GetComponent<Animator>().SetBool("open", true);
            Debug.Log("show and animate");
            //transform.parent.GetComponent<Animator>().Play("Show");

            if (transform.GetComponent<Button>() != null)
                transform.GetComponent<Button>().interactable = true;
        }
        else
        {
            Debug.Log("show");
            gameObject.SetActive(true);
        }
    }
    public virtual void OnHide()
    {
        if (transform.parent.GetComponent<Animator>() != null)
        {
            transform.parent.GetComponent<Animator>().SetBool("open", false);
            //transform.parent.GetComponent<Animator>().Play("Hide");
            Debug.Log("hide and animate");
            if (transform.GetComponent<Button>() != null)
                transform.GetComponent<Button>().interactable = false;
        }
        else
        {
            Debug.Log("hide");
            gameObject.SetActive(false);
        }
    }
}
