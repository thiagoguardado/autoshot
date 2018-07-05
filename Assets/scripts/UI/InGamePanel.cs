using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGamePanel : MonoBehaviour
{

    protected InGamePanelController panelController;
    public AudioClip panelOpenedClip;


    public void Init(InGamePanelController holder)
    {
        panelController = holder;

        AudioManager.Instance.PlaySFX(panelOpenedClip);
    }

    public void DestroyPanel()
    {
        Destroy(gameObject);
    }
}