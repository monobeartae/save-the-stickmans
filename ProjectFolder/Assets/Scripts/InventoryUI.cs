using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    PlayerInventory inventory;
    Transform itemSlotContainer;
    Transform itemSlotTemplate;
    PlayerMovement player;

    private float overlay_max_offset_y = 0.0f;
    private float overlay_min_offset_y = 50.0f;
    private Color unusable_tint = new Color(1.0f, 0.0f, 0.0f, 0.2f);

    private void Awake()
    {
        itemSlotContainer = transform.Find("PlayerInven").transform.Find("ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplate");
    }

    public void SetPlayer(PlayerMovement player)
    {
        this.player = player;
    }

    public void SetInventory(PlayerInventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemChanged;
        UpdateInventoryItems();
    }

    private void Inventory_OnItemChanged(object sender, System.EventArgs e)
    {
        UpdateInventoryItems();
    }

    public void UpdateInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate)
            {
                continue;
            }
            else
            {
                Destroy(child.gameObject);
            }
        }

        int x = 0, y = 0;
        float itemSlotSize = 120f;

        foreach (Item item in inventory.getItemList())
        {
            RectTransform itemSlotRect = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRect.gameObject.SetActive(true);

            itemSlotRect.GetComponent<CodeMonkey.Utils.Button_UI>().ClickFunc = () =>
            {
                inventory.UseItem(item);
            };
            itemSlotRect.GetComponent<CodeMonkey.Utils.Button_UI>().MouseRightClickFunc = () =>
            {
                //drop item
                Item duplicateItem = new Item { itemType = item.itemType, amt = item.amt };
                inventory.RemoveItem(item);
                if (item.itemType > Item.Type.WEAPON_START
                && item.itemType < Item.Type.WEAPON_END)
                {
                    ItemPickup.DropWeapon(player.GetPos(), item);
                }
                else
                {
                    ItemPickup.DropItem(player.GetPos(), duplicateItem);
                }
            };
            
            itemSlotRect.anchoredPosition = new Vector2(x * itemSlotSize, y * itemSlotSize);
            Image image = itemSlotRect.Find("Image").GetComponent<Image>();
            image.sprite = item.GetSprite();
                
            Text _text = itemSlotRect.Find("amountText").GetComponent<Text>();
            
            Image overlay = itemSlotRect.Find("Overlay").GetComponent<Image>();
            overlay.gameObject.transform.localScale = new Vector3(1.0f, 1.0f - item.durability, 1.0f);
            if (item.durability <= 0)
                overlay.color = unusable_tint;
         


            Vector3 defaultPos = overlay.transform.localPosition;
            defaultPos.y = item.durability * (overlay_min_offset_y - overlay_max_offset_y);
            overlay.transform.localPosition = defaultPos;

            if (item.amt > 1)
            {
                _text.text = (item.amt).ToString();
            }
            else {
                _text.text = "";
            }
            
            x++;
            if (x > 1)
            {
                x = 0;
                y--;
            }
        }
    }
}
