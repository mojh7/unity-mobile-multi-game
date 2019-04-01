using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginPlaceholder : MonoBehaviour, IPointerClickHandler
{
    private Animator animator;
    private InputField inputField;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        inputField = this.GetComponent<InputField>();
    }

    private void OnDisable()
    {
        animator.Play("down");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        animator.Play("up");
    }

    public void OnEndEdit()
    {
        if (inputField.text.Trim() == "")
        {
            animator.Play("down");
        }
    }
}
