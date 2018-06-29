using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePanel : InGamePanel {

    public void Menu()
    {
        panelController.ClosePanel();
        panelController.Menu();
    }
}
