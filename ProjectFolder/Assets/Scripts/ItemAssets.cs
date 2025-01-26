using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Transform itemPickup;

    public Sprite item_Gun;
    public Sprite item_Eraser;
    public Sprite item_Money;
    public Sprite item_HealthPotion;
}
