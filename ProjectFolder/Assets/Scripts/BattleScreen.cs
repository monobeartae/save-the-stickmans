using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScreen : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerHealth playerHealth;
    public EnemyChase enemyChase;

    public void Run()
    {
        playerMovement.setState(PlayerMovement.PlayerState.Map);
    }

    public void Fight()
    {
        playerHealth.LoseHealth(1);
        enemyChase.LoseHealth(1);
    }

    public void Update()
    {
        if (enemyChase.getHealth() <= 0)
        {
            gameObject.SetActive(false);
            playerMovement.setState(PlayerMovement.PlayerState.Map);
        }
    }
}
