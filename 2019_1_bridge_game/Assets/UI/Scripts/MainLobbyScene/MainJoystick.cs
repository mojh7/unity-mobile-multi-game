using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//메인 로비 조이스틱 이미지 이동
public class MainJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image bg_Img;
    private Image joystick_img;
    private Vector3 inputVector;

    void Start()
    {
        bg_Img = GetComponent<Image>();
        joystick_img = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bg_Img.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bg_Img.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bg_Img.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2, pos.y * 2, 0);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            //조이스틱 이동
            joystick_img.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bg_Img.rectTransform.sizeDelta.x / 3), inputVector.y * (bg_Img.rectTransform.sizeDelta.y / 3));

        }
    }
    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        joystick_img.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float GetHorizontalValue()
    {
        return inputVector.x;
    }

    public float GetVerticalValue()
    {
        return inputVector.y;
    }
}