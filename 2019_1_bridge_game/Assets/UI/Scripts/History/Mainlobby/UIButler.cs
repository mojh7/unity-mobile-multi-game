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
        PlayerPrefs.SetInt("Tutorial_Start", PlayerPrefs.GetInt("Tutorial_Start", 0));
    }

    private void OnDisable()
    {
        butler_img.SetActive(false);
        scriptpanel.SetActive(false);
        illustrated.SetActive(false);
    }
    public void touched()
    {
        transform.parent.GetComponent<Animator>().SetBool("touch", true);
        Debug.Log("touched");
    }


    override 
    public void OnShow()
    {
        //debug용
       // PlayerPrefs.SetInt("Tutorial_Start", 0);
 
        if (PlayerPrefs.GetInt("Tutorial_Start") == 0)
        {
            PlayerPrefs.SetInt("Tutorial_Start", 1);
            butler_img.SetActive(false);
            scriptpanel.SetActive(false);
            illustrated.SetActive(false);

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
            transform.parent.GetComponent<Animator>().SetBool("first", false);
        }

        else
        {
            transform.parent.GetComponent<Animator>().SetBool("first", false);
            butler_img2.SetActive(false);
            firstscript.SetActive(false);
        }

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
