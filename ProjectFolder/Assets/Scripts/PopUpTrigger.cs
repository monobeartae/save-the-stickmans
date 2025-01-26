using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTrigger : MonoBehaviour
{
    public POPUP_ID PopUpID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameHandler.instance.popupManager.QueuePopUp(PopUpID);
            gameObject.SetActive(false);
        }
    }

}
