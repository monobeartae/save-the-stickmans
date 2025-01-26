using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector2 SpawnPos;
    public CHECKPOINT_ID ID;

    private Light checkpointLight;
    private bool is_unlocked = false;
    private PopUp unlock_popup;
    private bool within_range = false;

    void Start()
    {
        checkpointLight = transform.Find("Light").gameObject.GetComponent<Light>();
        checkpointLight.color = Color.red;
    }

  
    void Unlock()
    {
        is_unlocked = true;
        checkpointLight.color = Color.green;
        GameHandler.instance.sfxScript.PlayCheckpoint();
        GameHandler.instance.checkpointManager.AddCheckpoint(this);
        unlock_popup.Deactivate();
    }

    void Update()
    {
        if (is_unlocked)
            return;

        if (within_range && Input.GetKeyDown(KeyCode.E))
            Unlock();

    }

   

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")
            && !is_unlocked)
        {
            unlock_popup = GameHandler.instance.popupManager.ActivatePopUp(POPUP_ID.UNLOCK_CHECKPOINT);
            within_range = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")
           && !is_unlocked)
        {
            unlock_popup.Deactivate();
            within_range = false;
        }
    }
}

public enum CHECKPOINT_ID
{
    CHECKPOINT_0,
    CHECKPOINT_1,


    NUM_TOTAL
}
