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
        panelController.Menu();
    }

}
