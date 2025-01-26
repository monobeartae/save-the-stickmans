using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPopUp : PopUp
{

    public Image item_display;
    public Text item_name, item_desc;

    NPC npc;
    Item weapon;
   
    public void Init(NPC who, Item item)
    {
        this.weapon = item;
        this.npc = who;
       
        item_display.sprite = weapon.GetSprite();
        item_name.text = item.item_name;
        item_desc.text = item.item_desc; 
    }

    public void AcceptWeapon()
    {
        npc.WeaponTaken();
    }

    public void RejectWeapon()
    {
        npc.WeaponLeftBehind(); 
    }
}
