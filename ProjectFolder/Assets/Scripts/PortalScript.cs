using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    private PopUp popup;
    private bool within_range = false;
    public string levelName;

    void Update()
    {
        if (within_range && Input.GetKeyDown(KeyCode.E))
        {
            if (levelName == "CreditsMenuScene")
            {
                if (NPC.SAVED_NPC_COUNT >= 3)
                {
                    nextLevel();
                }
                else {
                    GameHandler.instance.sfxScript.PlayUnableToBuy();
                }
            }
            else if (levelName == "GameScene")
            {
                nextLevel();
            }
        }
    }

    void nextLevel()
    {
        GameHandler.instance.levelLoader.LoadNextLevel(levelName);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            popup = GameHandler.instance.popupManager.ActivatePopUp(POPUP_ID.PORTAL);
            within_range = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            popup.Deactivate();
            within_range = false;
        }
    }
}
