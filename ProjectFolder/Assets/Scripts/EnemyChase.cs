using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float speed = 3f;
    public float triggerBattleRadius = 3f;
    private Transform target;
    public int health = 3;
    public string enemyTag;
    public PlayerMovement playerMovement;
    public GameObject battleUI;
    bool battleState = false;

    private void Start()
    {
    }

    private void Update()
    {
        if (!battleState)
        {
            if (target != null)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, target.position, step);

                float distance = Vector2.Distance(target.position, transform.position);
                if (distance <= triggerBattleRadius)
                {
                    enemyTag = gameObject.tag;
                    //set the battle screen canvas overlay
                    playerMovement.setState(PlayerMovement.PlayerState.Dead);
                    battleUI.SetActive(true);
                    battleState = true;
                }
            }
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            target = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            target = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerBattleRadius);
    }
    public int getHealth()
    {
        return health;   
    }

    public void setHealth(int _health)
    {
        health = _health;
    }

    public void minusHealth(int _health)
    {
        health -= _health;
    }

    public string getTag()
    {
        return enemyTag;
    }

    public void LoseHealth(int _health)
    {
        health -= _health;
    }

    public int GetHealth()
    {
        return health;
    }
}
