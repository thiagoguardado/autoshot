using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGamePanelController : MonoBehaviour {

    private Animator Animator;
    public Transform panelAnchor;

    public InGamePanel pausePanelPrefab;
    public InGamePanel victoryPanelPrefab;
    public InGamePanel defeatPanelPrefab;

    private InGamePanel instantiatedPanel;

    private const string AnimatorOpenPanelBool = "panelOpened";
    private const string AnimatorClosedStateName = "PanelClosed";

    public bool hasPanelOpened { get { return instantiatedPanel != null; } }

    private void OnEnable()
    {
        GameManager.Instance.OnNotifyLevelFinished += OpenFinalPanel;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnNotifyLevelFinished -= OpenFinalPanel;
    }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void OpenPausePanel()
    {
        OpenPanel(pausePanelPrefab);
    }

    public void OpenVictoryPanel()
    {
        OpenPanel(victoryPanelPrefab);
    }

    public void OpenLosePanel()
    {
        OpenPanel(defeatPanelPrefab);
    }

    private void OpenPanel(InGamePanel panelPrefab)
    {

        if (instantiatedPanel != null)
            Destroy(instantiatedPanel.gameObject);

        instantiatedPanel = Instantiate(panelPrefab, panelAnchor.position, Quaternion.identity, panelAnchor);
        instantiatedPanel.Init(this);

        Animator.SetBool(AnimatorOpenPanelBool, true);
    }

    public void ClosePanel()
    {
        if (hasPanelOpened)
        {
            Animator.SetBool(AnimatorOpenPanelBool, false);

            StartCoroutine(WaitForAnimationEnd());
        }
    }

    private IEnumerator WaitForAnimationEnd()
    {
        while (!Animator.GetCurrentAnimatorStateInfo(0).IsName(AnimatorClosedStateName))
            yield return null;

        instantiatedPanel.DestroyPanel();
    }

    public void OpenFinalPanel(bool success)
    {
        if (success)
        {
            OpenVictoryPanel();
        }
        else {
            OpenLosePanel();
        }

    }


    public void Menu()
    {
        GameManager.Instance.LoadMenu();
    }

}
