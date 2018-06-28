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

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }


    private void OpenPanel(InGamePanel panelPrefab)
    {

        if (instantiatedPanel != null)
            Destroy(instantiatedPanel.gameObject);

        instantiatedPanel = Instantiate(panelPrefab, panelAnchor.position, Quaternion.identity, panelAnchor);
        instantiatedPanel.Init(this);

        Animator.SetBool(AnimatorOpenPanelBool, true);
    }

    private void ClosePanel()
    {
        Animator.SetBool(AnimatorOpenPanelBool, false);

        StartCoroutine(WaitForAnimationEnd());

    }

    private IEnumerator WaitForAnimationEnd()
    {
        while (!Animator.GetCurrentAnimatorStateInfo(0).IsName(AnimatorClosedStateName))
            yield return null;

        instantiatedPanel.DestroyPanel();
    }

    public void OpenPausePanel()
    {
        OpenPanel(pausePanelPrefab);
    }

    public void ClosePausePanel()
    {
        ClosePanel();
    }

    public void Menu()
    {

    }

}
