using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracingPanelUI : MonoBehaviour
{
    public static TracingPanelUI o;

    GameObject panelGO => transform.GetChild(0).gameObject;

    private void Awake()
    {
        o = this;
    }
    public void show()
    {
        panelGO.SetActive(true);
    }


    public void hide()
    {
        panelGO.SetActive(false);
    }


}
