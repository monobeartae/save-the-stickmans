using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3.0f;
    public Rigidbody2D rigidBody;
    Vector2 movement;
    public Animator animator;

    public Sprite health5;
    public Sprite health4;
    public Sprite health3;
    public Sprite health2;
    public Sprite health1;
    public Sprite dead;

    //player health
    PlayerHealth health;

    //player inventory
    PlayerInventory inventory;
    [SerializeField] private InventoryUI inventoryUI;

    bool invenSlideIn = false;
    public bool invenActive = false;
    bool invenSlideOut = false;
    float invenSlideOutTimer = 0.4f;

    Collider2D collide;
    bool withinPickupRange;

    public enum PlayerState
    {
        Map,
        Dead
    }

    public static PlayerState state;

    void Start()
    {
        inventory = new PlayerInventory(UseItem);
        inventoryUI.SetPlayer(this);
        inventoryUI.SetInventory(inventory);
        GameHandler.instance.playerInventory = inventory;
        GameHandler.instance.playerWeapon.AddUseItemFunction();
        health = GameHandler.instance.playerHealth;
        //state = PlayerState.Map;
        movement.x = 0;
        movement.y = 0;

        //ItemPickup.spawnItem(new Vector3(5, 5), new Item { itemType = Item.Type.Gun, amt = 1});
        //ItemPickup.spawnItem(new Vector3(-5, 5), new Item { itemType = Item.Type.Eraser, amt = 1});
        //ItemPickup.spawnItem(new Vector3(5, -5), new Item { itemType = Item.Type.HealthPotion, amt = 1});
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collide = collision;
        ItemPickup _item = collision.GetComponent<ItemPickup>();
        if (_item != null)
        {
            withinPickupRange = true;
        }
    }

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.Type.HealthPotion:
                health.AddHealth(1);
                inventory.RemoveItem(new Item { itemType = Item.Type.HealthPotion, amt = 1 });
                break;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        withinPickupRange = false;
    }

    public PlayerState getState()
    {
        return state;
    }

    public void setState(PlayerState _state)
    {
        state = _state;
    }

    void Update()
    {
        if (GameStateManager.IN_CUTSCENE)
            return;

        if (state == PlayerState.Dead)
        {
            movement.x = 0;
            movement.y = 0;
        }
        else
        {
            movement.x = Input.GetAxisRaw("Horizontal"); //returns -1 if moving left, returns 1 if moving right
            movement.y = Input.GetAxisRaw("Vertical"); //returns -1 if moving down, returns 1 if moving up
            ShowInventory();
            PickupItem();
        }

    }

    void FixedUpdate()
    {
        ChangeSprite();

        if (GameStateManager.IN_CUTSCENE)
            return;

        //sprite's movement
        if (movement.x != 0 || movement.y != 0)
        {
            rigidBody.MovePosition(rigidBody.position + movement * speed * Time.fixedDeltaTime);
            animator.SetTrigger("Moving");
        }
        else
        {
            animator.SetTrigger("Idle");
        }
    }
    void ChangeSprite()
    {
        if (health.GetHealth() == 5)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = health5;
            this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.5625f, 1.5625f);
        }
        else if (health.GetHealth() == 4)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = health4;
            this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.5625f, 1.5625f);
        }
        else if (health.GetHealth() == 3)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = health3;
            this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.01f, 0);
            this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.53f, 1.43f);
        }
        else if (health.GetHealth() == 2)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = health2;
            this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.06f, -0.05f);
            this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.4f, 1.24f);
        }
        else if (health.GetHealth() == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = health1;
            this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.03f, -0.06f);
            this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.37f, 0.75f);
        }
        else if (health.GetHealth() == 0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = dead;
            this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.03f, -0.03f);
            this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.89f, 0.87f);
        }
    }

    void ShowInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) //activates inventory
        {
            GameHandler.instance.sfxScript.PlayInventoryOpen();
            invenSlideIn = !invenSlideIn;
            if (invenActive)
            {
                GameHandler.instance.inventoryUI.UpdateInventoryItems();
                invenSlideOut = true;
                invenActive = false;
            }
        }

        if (invenSlideIn)
        {
            inventoryUI.transform.Find("PlayerInven").GetComponent<Animator>().ResetTrigger("SlideOut");
            inventoryUI.transform.Find("PlayerInven").GetComponent<Animator>().SetTrigger("SlideIn");
            invenActive = true;
        }
        if (invenSlideOut)
        {
            inventoryUI.transform.Find("PlayerInven").GetComponent<Animator>().ResetTrigger("SlideIn");
            inventoryUI.transform.Find("PlayerInven").GetComponent<Animator>().SetTrigger("SlideOut");
            invenSlideOutTimer -= Time.deltaTime;
            if (invenSlideOutTimer <= 0)
            {
                invenSlideOut = false;
                invenSlideOutTimer = 0.4f;
            }
        }
    }

    void PickupItem()
    {
        if (withinPickupRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameHandler.instance.sfxScript.PlayPickupItem();
                ItemPickup _item = collide.GetComponent<ItemPickup>();
                Debug.Log("Item pickedup");
                inventory.AddItem(_item.GetItem());
                _item.DestroySelf();
            }
        }
    }

    public Vector3 GetPos()
    {
        return gameObject.transform.position;
    }

    public void CloseInven()
    {
        if (invenActive)
        {
            invenSlideIn = !invenSlideIn;
            GameHandler.instance.inventoryUI.UpdateInventoryItems();
            invenSlideOut = true;
            invenActive = false;
        }
    }

    public void stopMoving()
    {
        movement.x = 0;
        movement.y = 0;
    }
}
