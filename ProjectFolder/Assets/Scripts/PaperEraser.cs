using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperEraser : MonoBehaviour
{
    Vector3 direction;

    float velocity = 0.0f;
    float initial_velocity = 15.0f;
    float elastic_constant = 15.0f;

    Eraser eraser_parent;

    bool has_returned = false;
    bool collided = false;
    ERASER_STATE state = ERASER_STATE.DEFAULT;

    enum ERASER_STATE
    {
        DEFAULT,
        GOING,
        RETURNING,

        NUM_TOTAL
    }

    void Start()
    {
        
    }

    public void InitPaperEraser(Vector2 dir, Eraser pos)
    {
        direction = new Vector3(dir.x, dir.y, 0);
        state = ERASER_STATE.GOING;
        has_returned = false;
        collided = false;
        velocity = initial_velocity;
        eraser_parent = pos;
    }

    void Update()
    {
        if (GameStateManager.IN_CUTSCENE || state == ERASER_STATE.DEFAULT)
            return;



        switch (state)
        {
            case ERASER_STATE.GOING:
                velocity -= elastic_constant * Time.deltaTime;

                GameHandler.instance.sfxScript.PlayEraser();

                if (velocity <= 0.0f || collided)
                {
                    velocity = 0.0f;
                    state = ERASER_STATE.RETURNING;
                }
                break;
            case ERASER_STATE.RETURNING:
                Vector3 targetPos = eraser_parent.transform.position;
                direction = (targetPos - transform.position).normalized;
             
                velocity += elastic_constant * Time.deltaTime;
                if (Vector3.Distance(transform.position, targetPos) <= 0.1f)
                {
                    has_returned = true;
                    state = ERASER_STATE.DEFAULT;
                }
                break;
        }

        transform.position += velocity * direction * Time.deltaTime;
    }

    public bool GetHasReturned()
    {
        return has_returned;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SolidObject"))
        {
            collided = true;
        }
        else if (collision.CompareTag("Enemy1") && state != ERASER_STATE.DEFAULT)
        {
            WaterGlassEnemy enemy = collision.gameObject.GetComponent<WaterGlassEnemy>();
            enemy.TakeDamage();
        }
    }
}
