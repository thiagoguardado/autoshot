using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : InGamePanel {


    public void Continue()
    {
        GameManager.Instance.Pause();
    }


    public void Menu()
    {
        GameManager.Instance.Pause();
        panelController.Menu();
    }

}
