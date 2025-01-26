using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public static int TOTAL_NPC_COUNT = 0;
    public static int SAVED_NPC_COUNT = 0;

    public GameObject BulletPrefab;
    public GameObject EraserPrefab;

    private bool is_released = false;
    private bool is_fading = false;
    private PopUp release_popup;
    private bool within_range = false;

    private Animator animator;

    private Item weapon;
    private Vector3 weapon_scale = new Vector3(0.15f, 0.15f, 1.0f);
    private WeaponPopUp weapon_popup;

    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject player_weapon = transform.Find("Item").gameObject;
  
     
        Item.Type type = (Item.Type)Random.Range((int)Item.Type.WEAPON_START + 1, (int)Item.Type.WEAPON_END);
        switch (type)
        {
            case Item.Type.Gun:
                Gun gun = player_weapon.gameObject.AddComponent<Gun>();
                gun.bullet = BulletPrefab;
                gun.Init();
                weapon = gun;
                break;
            case Item.Type.Eraser:
                Eraser eraser = player_weapon.gameObject.AddComponent<Eraser>();
                eraser.eraser_prefab = EraserPrefab;
                eraser.Init();
                weapon = eraser;
                break;
            default:
                break;
        }
        weapon.itemType = type;
        weapon.amt = 1;
        weapon.gameObject.SetActive(false);

        SpriteRenderer weaponSprite = transform.Find("Item").gameObject.GetComponent<SpriteRenderer>();
        weaponSprite.sprite = weapon.GetSprite();

        TOTAL_NPC_COUNT++;
    }

    void Update()
    {
        if (GameStateManager.IN_CUTSCENE || is_fading)
            return;

     
        if (is_released)
        {
            if (!AnimatorIsPlaying("Release"))
                PassWeapon();
            return;
        }
       
           

        if (within_range && Input.GetKeyDown(KeyCode.E))
            Release();
    }

    void Release()
    {
        GameHandler.instance.sfxScript.PlayCheckpoint();
        is_released = true;
        release_popup.Deactivate();
        animator.SetTrigger("Release");
        SAVED_NPC_COUNT++;
        
    }

    void PassWeapon()
    {
        weapon_popup = (WeaponPopUp)(GameHandler.instance.popupManager.QueuePopUp(POPUP_ID.PASS_WEAPON));
        weapon_popup.Init(this, weapon);
    }

    public void WeaponTaken()
    {
        // Weapon Give things
        weapon.transform.SetParent(null);
        GameHandler.instance.playerInventory.AddItem(weapon);
        animator.SetTrigger("Disappear");
        weapon_popup.Deactivate();
        Disappear();
    }

    public void WeaponLeftBehind()
    {
        animator.SetTrigger("Disappear");
        weapon_popup.Deactivate();
        Disappear();
        ItemPickup.spawnItem(transform.position, weapon);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")
            && !is_released)
        {
            release_popup = GameHandler.instance.popupManager.ActivatePopUp(POPUP_ID.SAVE_NPC);
            within_range = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")
           && !is_released)
        {
            release_popup.Deactivate();
            within_range = false;
        }
    }

    bool AnimatorIsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length >
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    bool AnimatorIsPlaying(string stateName)
    {
        AnimatorClipInfo[] animatorinfo = animator.GetCurrentAnimatorClipInfo(0);
        return AnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    void Disappear()
    {
        is_fading = true;
        StartCoroutine(DestroyOnAnimEnd());
    }

    IEnumerator DestroyOnAnimEnd()
    {

        yield return new WaitForSeconds(2.0f);
        //while (AnimatorIsPlaying())
        //{
        //    //Debug.Log("Disappear Anim Not Playing, Waiting...");
        //    yield return null;
        //}

        Debug.Log("Destroying NPC");
        Destroy(gameObject);
    }
}
