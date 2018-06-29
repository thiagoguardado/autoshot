using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryPanel : InGamePanel {

    public void Menu()
    {
        AudioManager.Instance.PlayButtonPressAudio();

        panelController.ClosePanel();
        panelController.Menu();
    }

}
