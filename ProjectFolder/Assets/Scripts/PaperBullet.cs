using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBullet : MonoBehaviour
{

    public Vector2 direction;


    Rigidbody2D bullet_rb;
    float bullet_speed = 50.0f;

    void Start()
    {
        bullet_rb = GetComponent<Rigidbody2D>();
       
    }

    void Update()
    {
        bullet_rb.MovePosition(bullet_rb.position + direction * bullet_speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SolidObject"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy1"))
        {
            Destroy(gameObject);
            WaterGlassEnemy enemy = collision.gameObject.GetComponent<WaterGlassEnemy>();
            enemy.TakeDamage();
        }
    }
}
