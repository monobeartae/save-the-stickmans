using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Merchant : MonoBehaviour
{
    private PopUp popup;
    private bool within_range = false;
    int currencyAmt = 0;

    void Start() {
    }


    void Update()
    {
        currencyAmt = GameHandler.instance.playerInventory.getCurrencyAmt();

        if (within_range && Input.GetKeyDown(KeyCode.E) && currencyAmt >= 2)
        {
            BuyPotion();
        }
        else if (within_range && Input.GetKeyDown(KeyCode.E) && currencyAmt < 2)
        {
            GameHandler.instance.sfxScript.PlayUnableToBuy();
        }
    }

    void BuyPotion()
    {
        GameHandler.instance.sfxScript.PlayBuyItem();
        GameHandler.instance.playerInventory.AddItem(new Item { itemType = Item.Type.HealthPotion, amt = 1 });
        GameHandler.instance.playerInventory.RemoveItem(new Item { itemType = Item.Type.Currency, amt = 1 });
        GameHandler.instance.playerInventory.RemoveItem(new Item { itemType = Item.Type.Currency, amt = 1 });
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            popup = GameHandler.instance.popupManager.ActivatePopUp(POPUP_ID.MERCHANT_NPC);
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
