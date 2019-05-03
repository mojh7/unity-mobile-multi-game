using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButler : UIControl
{
    [SerializeField] private GameObject butler_img;
    [SerializeField] private GameObject butler_img2;
    [SerializeField] private GameObject scriptpanel;
    [SerializeField] private GameObject illustrated;
    [SerializeField] private GameObject firstscript;


    private void Start()
    {
        //초기실행 확인
        PlayerPrefs.SetInt("Tutorial_Start", PlayerPrefs.GetInt("Tutorial_Start", 0));
        //debug 용
        //PlayerPrefs.SetInt("Tutorial_Start", 0);
    }

    //첫 실행 시 터치 여부 확인
    public void touched()
    {
        transform.parent.GetComponent<Animator>().SetBool("touch", true);
        Debug.Log("touched");
    }

    //UIControl OnShow, OnHide 오버라이드
    override 
    public void OnShow()
    {

        //첫 실행 시
        if (PlayerPrefs.GetInt("Tutorial_Start") == 0)
        {
            Debug.Log("first exe");
            
            transform.parent.GetComponent<Animator>().SetBool("first", true);
            if (transform.parent.GetComponent<Animator>() != null)
            {
                transform.parent.GetComponent<Animator>().SetBool("open", true);
                Debug.Log("show and animate");

                if (transform.GetComponent<Button>() != null)
                    transform.GetComponent<Button>().interactable = true;
            }
            else
            {
                Debug.Log("show");
                gameObject.SetActive(true);
            }

        }

        //첫 실행X
        else
        {
            transform.parent.GetComponent<Animator>().SetBool("first", false);

            if (transform.parent.GetComponent<Animator>() != null)
            {
                transform.parent.GetComponent<Animator>().SetBool("open", true);
                Debug.Log("show and animate");

                if (transform.GetComponent<Button>() != null)
                    transform.GetComponent<Button>().interactable = true;
            }
            else
            {
                Debug.Log("show");
                gameObject.SetActive(true);
            }
        }


    }

    override
    public void OnHide()
    {
        //첫실행 이후 도감 종료
        if (PlayerPrefs.GetInt("Tutorial_Start") == 0)
        {
            transform.parent.GetComponent<Animator>().SetBool("first", false);
            PlayerPrefs.SetInt("Tutorial_Start", 1);
            butler_img2.SetActive(false);
            firstscript.SetActive(false);
        }

        if (transform.parent.GetComponent<Animator>() != null)
        {
            transform.parent.GetComponent<Animator>().SetBool("open", false);
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
