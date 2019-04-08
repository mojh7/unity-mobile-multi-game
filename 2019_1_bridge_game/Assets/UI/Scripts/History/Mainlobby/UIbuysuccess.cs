using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIbuysuccess : UIControl
{
    [SerializeField] private bool is_on;
    private void OnEnable()
    {
        is_on = true;
        Invoke("panelfalse", 1);
    }
    public void panelfalse()
    {
        is_on = false;
        OnHide();
    }

    public bool GetIs_on()
    {
        return is_on;
    }
    public void SetIs_on(bool ison)
    {
        is_on = ison;
    }

}
