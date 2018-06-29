using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryPanel : InGamePanel {

    public void Menu()
    {
        panelController.ClosePanel();
        panelController.Menu();
    }

}
