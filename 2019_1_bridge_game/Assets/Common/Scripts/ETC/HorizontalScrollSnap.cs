using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ScrollRect))]
public class HorizontalScrollSnap : MonoBehaviour
{
    float[] points;
    [Tooltip("how many screens or pages are there within the content (steps)")]
    public int screens = 1;
    [Tooltip("How quickly the GUI snaps to each panel")]
    public float snapSpeed;
    public float inertiaCutoffMagnitude;
    float stepSize;

    ScrollRect scroll;
    bool LerpH;
    [SerializeField]
    public float targetH;


    bool dragInit = true;
    int dragStartNearest;
  
    void Start()
    {
        scroll = gameObject.GetComponent<ScrollRect>();
        scroll.inertia = true;

        InitPoint();
    }

    void Update()
    {
        if (LerpH)
        {
            scroll.horizontalNormalizedPosition = Mathf.Lerp(scroll.horizontalNormalizedPosition, targetH, snapSpeed * Time.deltaTime);
            if (Mathf.Approximately(scroll.horizontalNormalizedPosition, targetH)) LerpH = false;
        }
    }

    public void InitPoint()
    {
        if (screens > 0)
        {
            points = new float[screens];
            stepSize = 1 / (float)(screens - 1);

            for (int i = 0; i < screens; i++)
            {
                points[i] = i * stepSize;
            }
        }
        else
        {
            points[0] = 0;
        }
    }

    public void DragEnd()
    {
        int target = FindNearest(scroll.horizontalNormalizedPosition, points);
        
        if (target == dragStartNearest && scroll.velocity.sqrMagnitude > inertiaCutoffMagnitude * inertiaCutoffMagnitude)
        {
            if (scroll.velocity.x < 0)
            {
                target = dragStartNearest + 1;
            }
            else if (scroll.velocity.x > 1)
            {
                target = dragStartNearest - 1;
            }
            target = Mathf.Clamp(target, 0, points.Length - 1);
        }

        if (scroll.horizontal && scroll.horizontalNormalizedPosition > 0f && scroll.horizontalNormalizedPosition < 1f)
        {
            targetH = points[target];
            LerpH = true;
        }

        dragInit = true;
    }

    public void OnDrag()
    {
        if (dragInit)
        {
            dragStartNearest = FindNearest(scroll.horizontalNormalizedPosition, points);
            dragInit = false;
        }

        LerpH = false;
    }

    int FindNearest(float f, float[] array)
    {
        float distance = Mathf.Infinity;
        int output = 0;
        for (int index = 0; index < array.Length; index++)
        {
            if (Mathf.Abs(array[index] - f) < distance)
            {
                distance = Mathf.Abs(array[index] - f);
                output = index;
            }
        }
        return output;
    }

    public float GetTargetH
    {
        get { return targetH; }
        set { targetH = value; }
    }

    public void SetPositionScreen(int index)
    {
        LerpH = true;
        InitPoint();
        if (index < 0) index = 0;
        targetH = points[index];
    }

    public void SetScreenCount(int count)
    {
        screens = count;
        InitPoint();
    }
}