using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGamePanel : MonoBehaviour
{

    protected InGamePanelController panelController;

    public void Init(InGamePanelController holder)
    {
        panelController = holder;
    }

    public void DestroyPanel()
    {
        Destroy(gameObject);
    }
}