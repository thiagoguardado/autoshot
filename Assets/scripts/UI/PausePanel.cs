using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : InGamePanel {


    public void Continue()
    {
        AudioManager.Instance.PlayButtonPressAudio();

        GameManager.Instance.Pause();
    }


    public void Menu()
    {
        AudioManager.Instance.PlayButtonPressAudio();

        GameManager.Instance.Pause();
        panelController.Menu();
    }

}
