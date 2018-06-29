using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : InGamePanel {


    public void Continue()
    {
        GameManager.Instance.Pause();
        panelController.ClosePanel();
    }


    public void Menu()
    {
        GameManager.Instance.Pause();
        panelController.ClosePanel();
        panelController.Menu();
    }

}
