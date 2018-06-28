using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {


    [System.Serializable]
    public class MenuButton
    {
        public int id;
        public Button button; 
    }

    public MenuButton[] menuButtons;


    private void Awake()
    {
        UpdateButtonsState();
    }

    void UpdateButtonsState()
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            Level lvl = GameManager.Instance.LevelsManager.GetLevelById(menuButtons[i].id);
            if (lvl != null)
            {
                menuButtons[i].button.interactable = lvl.isOpened;
            }
 
        }

    }

    public void StartLevel(Button caller)
    {

        for (int i = 0; i < menuButtons.Length; i++)
        {
            if (menuButtons[i].button == caller)
            {
                GameManager.Instance.StartLevel(menuButtons[i].id);
            }
        }

    }

}
