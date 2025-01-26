using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGlassEnemy : MonoBehaviour
{

    public Vector3 dir;
    public GameObject ice_prefab;
    public Sprite dead_sprite;
    

    ENEMY_STATES state;
    int HP = 3;
    float ALERT_RADIUS = 10.0f;
    float PATROL_SPEED = 1.0f;
    float PATROL_INTERVAL = 2.0f;
    float ATTACK_INTERVAL = 1.5f;
    float patrol_timer = 0.0f;
    float attack_timer = 0.0f;

    Animator animator;

    enum ENEMY_STATES   
    {
        PATROL,
        ATTACK,

        NUM_TOTAL
    }
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

   
    void Update()
    {

        if (GameStateManager.IN_CUTSCENE)
            return;

        switch (state)
        {
            case ENEMY_STATES.PATROL:
                patrol_timer += Time.deltaTime;
                if (patrol_timer >= PATROL_INTERVAL)
                {
                    patrol_timer = 0.0f;
                    dir *= -1;
                }

                transform.position += dir * PATROL_SPEED * Time.deltaTime;
                if (CheckPlayerWithinSight())
                    state = ENEMY_STATES.ATTACK;
                break;
            case ENEMY_STATES.ATTACK:
                if (!CheckPlayerWithinSight())
                {
                    state = ENEMY_STATES.PATROL;
                    break;
                }
                attack_timer += Time.deltaTime;
                if (attack_timer >= ATTACK_INTERVAL)
                {
                    attack_timer = 0.0f;
                    LaunchIceCube();
                }
                break;
            default:
                break;
        }
    }

    bool CheckPlayerWithinSight()
    {
        Vector2 PlayerPos = new Vector2(GameHandler.instance.player.transform.position.x, GameHandler.instance.player.transform.position.y);
        Vector2 EnemyPos = new Vector2(transform.position.x, transform.position.y);

        float displacement = Vector2.Distance(PlayerPos, EnemyPos);
        if (displacement > ALERT_RADIUS)
            return false;

        LayerMask wall_mask = LayerMask.GetMask("SolidObject");
        RaycastHit2D hitData;
        hitData = Physics2D.Linecast(EnemyPos, PlayerPos, wall_mask);
        if (hitData.collider != null)
            return false;

   
        return true;
    }

    void LaunchIceCube()
    {
        IceCube iceCube = Instantiate(ice_prefab, transform.position, transform.rotation).GetComponent<IceCube>();
        iceCube.InitIceCube(GameHandler.instance.player.transform.position);
    }

    public void TakeDamage()
    {
        HP -= 1;
        if (HP <= 0)
        {

            GameHandler.instance.sfxScript.PlayEnemyDie();
            GetComponent<SpriteRenderer>().sprite = dead_sprite;
            Destroy(this);
            Destroy(GetComponent<Animator>());
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(GetComponent<Rigidbody2D>());
        }
        else
            animator.SetTrigger("Ouch");
    }
}
