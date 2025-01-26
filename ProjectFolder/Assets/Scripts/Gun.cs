using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Item
{
    public GameObject bullet;

    Vector2 gunViewportPos;


    public void Init()
    {
        item_name = "Paper Gun";
        item_desc = "Left mouse click anywhere to fire a paper ball in that direction!";
    }

    void OnEnable()
    {
        //   player_transform = transform.parent;
    }

    public override void UpdateWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && durability > 0.0f)
        {
            GameHandler.instance.sfxScript.PlayGunshot();
            PaperBullet newbullet = Instantiate(bullet, transform.position, transform.rotation).GetComponent<PaperBullet>();
            newbullet.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 10;
            Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            gunViewportPos = Camera.main.WorldToViewportPoint(this.transform.position);
            Vector2 dir = new Vector2(mousePos.x, mousePos.y) - gunViewportPos;
            newbullet.SetDirection(dir.normalized);

            durability -= 0.1f;

        }
    }


}
