using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapon : MonoBehaviour
{
    Item currentWeapon = null;
    public Vector3 weapon_offset;
    public Vector3 weapon_scale;

    private float overlay_max_offset_y = 0.0f;
    private float overlay_min_offset_y = 3.6f;
    private float overlay_max_scale = 7.5f;
    private Color unusable_tint = new Color(1.0f, 0.0f, 0.0f, 0.2f);

    public void AddUseItemFunction()
    {
        GameHandler.instance.playerInventory.AddUseItemAction(UseItem);
    }

    void Update()
    {
        if (GameStateManager.IN_CUTSCENE
            || GameHandler.instance.playerMovement.invenActive)
            return;


        if (currentWeapon == null)
            return;


        currentWeapon.UpdateWeapon();

        SpriteRenderer overlay = currentWeapon.transform.Find("Overlay").GetComponent<SpriteRenderer>();
        overlay.gameObject.transform.localScale = new Vector3(overlay_max_scale, (1.0f - currentWeapon.durability) * overlay_max_scale, 1.0f);

        if (currentWeapon.durability <= 0.0f)
        {
            overlay.color = unusable_tint;
            overlay.gameObject.transform.localScale = new Vector3(overlay_max_scale, overlay_max_scale, 1.0f);
        }



        Vector3 defaultPos = overlay.transform.localPosition;
        defaultPos.y = currentWeapon.durability * (overlay_min_offset_y - overlay_max_offset_y);
        overlay.transform.localPosition = defaultPos;
    }


    private void UseItem(Item item)
    {
       
        switch (item.itemType)
        {
            case Item.Type.Gun:
                UnEquip();
                Equip(item);
                GameHandler.instance.playerInventory.RemoveItem(item);
                break;
            case Item.Type.Eraser:
                UnEquip();
                Equip(item);
                GameHandler.instance.playerInventory.RemoveItem(item);
                break;
        }
    }

    private void Equip(Item weapon)
    {
        // Attach to Player
        weapon.gameObject.SetActive(true);
        currentWeapon = weapon;
        currentWeapon.transform.SetParent(this.transform);
        currentWeapon.transform.localPosition = weapon_offset;
        currentWeapon.transform.localScale = weapon_scale;
        currentWeapon.GetComponent<SpriteRenderer>().sortingOrder = 5;
        SpriteRenderer overlay = currentWeapon.transform.Find("Overlay").GetComponent<SpriteRenderer>();
        overlay.sortingOrder = 6;
    }

    private void UnEquip()
    {
        if (currentWeapon == null)
            return;

        currentWeapon.GetComponent<SpriteRenderer>().sortingOrder = 2;
        SpriteRenderer overlay = currentWeapon.transform.Find("Overlay").GetComponent<SpriteRenderer>();
        overlay.sortingOrder = 3;
        GameHandler.instance.playerInventory.AddItem(currentWeapon);
        currentWeapon = null;
    }
}
