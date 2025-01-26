using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private PopUp popup; 

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            popup = GameHandler.instance.popupManager.ActivatePopUp(POPUP_ID.ITEM);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            popup.Deactivate();
        }
    }

    public static ItemPickup spawnItem(Vector3 pos, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.itemPickup, pos, Quaternion.identity);
        ItemPickup _item = transform.GetComponent<ItemPickup>();
        _item.SetItem(item);

        return _item;
    }

    private Item item;
    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
    }

    public Item GetItem() {
        return item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public static ItemPickup DropItem(Vector3 dropPos, Item item)
    {
        ItemPickup _item = spawnItem(dropPos, item);
        return _item;
    }

    public static void DropWeapon(Vector3 dropPos, Item item)
    {
        item.transform.SetParent(null);
        item.transform.position = dropPos;
        item.gameObject.AddComponent<ItemPickup>();
    }

}
