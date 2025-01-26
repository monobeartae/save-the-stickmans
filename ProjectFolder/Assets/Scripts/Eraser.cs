using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eraser : Item
{
    public GameObject eraser_prefab;
    public Sprite empty_eraser_sprite;

    
    private Sprite eraser_sprite;
    private PaperEraser eraser_boomerang;

    private ERASER_STATE state = ERASER_STATE.DEFAULT;
    private Vector2 xAxis = new Vector2(1.0f, 0.0f);

    enum ERASER_STATE
    {
        DEFAULT,
        AIMING,
        RELEASED,

        NUM_TOTAL
    }

    public void Init()
    {
        item_name = "Boomerang Eraser";
        item_desc = "Hold Left Mouse Click to Pull Back and Aim the eraser and release to fire! Once it has reached a maximum distance it will come back to you.";

        eraser_boomerang = Instantiate(eraser_prefab, transform.position, transform.rotation).GetComponent<PaperEraser>();
        eraser_boomerang.transform.SetParent(this.transform);
    }

    

    public override void UpdateWeapon()
    {
       

        switch (state)
        {
            case ERASER_STATE.DEFAULT:
                if (Input.GetKey(KeyCode.Mouse0) && durability > 0.0f)
                {
                    state = ERASER_STATE.AIMING;
                    eraser_boomerang.GetComponent<SpriteRenderer>().sortingOrder = 10;
                    // empty sprite
                }
                break;
            case ERASER_STATE.AIMING:
                Vector2 eraserPos = new Vector2(transform.position.x, transform.position.y);
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mouseWorldPos = new Vector2(mousePos.x, mousePos.y);
                Vector2 dir = mouseWorldPos - eraserPos;
                dir.Normalize();


                float max_radius = 1.0f;
                //Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
                float dis = Vector3.Distance(mouseWorldPos, eraserPos);
                if (dis > max_radius)
                    dis = max_radius;
               
                Vector2 pos = eraserPos + dis * dir;
                eraser_boomerang.transform.position = pos;
                if (!Input.GetKey(KeyCode.Mouse0))
                {
                    eraser_boomerang.InitPaperEraser(-1 * dir, this);
                    eraser_boomerang.transform.SetParent(null);
                    state = ERASER_STATE.RELEASED;
                    durability -= 0.2f;
                  
                }
                break;
            case ERASER_STATE.RELEASED:
                if (eraser_boomerang.GetHasReturned())
                {
                    state = ERASER_STATE.DEFAULT;
                    eraser_boomerang.transform.SetParent(this.transform);
                    eraser_boomerang.transform.localPosition = Vector3.zero;
                    eraser_boomerang.GetComponent<SpriteRenderer>().sortingOrder = 0;
                }    
                break;
            default:
                break;
        }
    }
}
