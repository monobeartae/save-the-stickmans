using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item : MonoBehaviour
{
    public enum Type { 
        HealthPotion,
        Currency,
        WEAPON_START = 10,
        Eraser,
        Gun,
        WEAPON_END
    }

    public Type itemType;
    public int amt;
    public string item_name, item_desc;

    public float durability = 1.0f;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case Type.Gun:
                return ItemAssets.Instance.item_Gun;
            case Type.Eraser:
                return ItemAssets.Instance.item_Eraser;
            case Type.HealthPotion:
                return ItemAssets.Instance.item_HealthPotion;
            case Type.Currency:
                return ItemAssets.Instance.item_Money;
        }
    }

    public bool isStackable()
    {
        switch (itemType)
        {
            default:
            case Type.Gun:
            case Type.Eraser:
                return false;
            case Type.HealthPotion:
            case Type.Currency:
                return true;

        }
    }

    public virtual void UpdateWeapon()
    {

    }
}
