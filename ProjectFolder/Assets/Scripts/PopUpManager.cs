using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public List<PopUp> all_popupList = new List<PopUp>();

    // Queued PopUps (Dialogues, etc)
    PopUp current_popup = null;
    List<PopUp> waiting_popupList = new List<PopUp>();

    // Custom PopUps... 
    List<PopUp> active_popupList = new List<PopUp>();
    List<PopUp> temp_remove = new List<PopUp>();



  
    void Update()
    {
        //TESTING
        if (Input.GetKeyDown(KeyCode.P))
            QueuePopUp(POPUP_ID.WATER_INTRO);

        

        UpdateQueue();


        
        foreach (PopUp popup in active_popupList)
        {
            if (popup.GetState() == POPUP_ANIM_STATE.COMPLETE)
            {
                popup.gameObject.SetActive(false);
                temp_remove.Add(popup);
            }
        }
        foreach (PopUp popup in temp_remove)
        {
            active_popupList.Remove(popup);
        }
        temp_remove.Clear();

    }


    void UpdateQueue()
    {
        if (current_popup == null)
            return;
        else if (current_popup.GetState() == POPUP_ANIM_STATE.COMPLETE)
            NextPopUp();
    }

    public PopUp ActivatePopUp(POPUP_ID popup_id)
    {
        PopUp toAdd = GetPopUp(popup_id);
        if (toAdd == null)
        {
            Debug.Log("PopUp of this ID could not be found!");
            return null;
        }

        active_popupList.Add(toAdd);
        toAdd.gameObject.SetActive(true);
        return toAdd;
    }

    public PopUp QueuePopUp(POPUP_ID popup_id)
    {
        PopUp toAdd = GetPopUp(popup_id);
        if (toAdd == null)
        {
            Debug.Log("PopUp of this ID could not be found!");
            return toAdd;
        }
       

        if (current_popup == null)
        {
            current_popup = toAdd;
            toAdd.gameObject.SetActive(true);
            GameStateManager.IN_CUTSCENE = true;

        }
        else
        {
            waiting_popupList.Add(toAdd);
        }
        return toAdd;
    }

    PopUp GetPopUp(POPUP_ID popup_id)
    {
        foreach (PopUp popup in all_popupList)
        {
            if (popup.ID == popup_id)
                return popup;
        }

        return null;
    }

    void NextPopUp()
    {
        current_popup.gameObject.SetActive(false);
    
        if (waiting_popupList.Count <= 0)
        {
            current_popup = null;
            GameStateManager.IN_CUTSCENE = false;
        }
        else
        {
            current_popup = waiting_popupList[0];
            waiting_popupList.Remove(current_popup);
            current_popup.gameObject.SetActive(true);
        }
    }
}
