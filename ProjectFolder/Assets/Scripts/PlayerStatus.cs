using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{


    private float wet_rate = 0.1f;
    private float wet_limit = 0.8f;
    private float wet_amount = 0.0f;

    private Animator animController;

    void Start()
    {
        animController = GetComponent<Animator>();
    }
    public void ResetPlayerStatus()
    {
        wet_amount = 0.0f;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (1 - wet_amount));
    }

    void OnTriggerStay2D(Collider2D collision)
    {

        if (GameStateManager.IN_CUTSCENE)
            return;

        if (collision.gameObject.CompareTag("Wet"))
        {
            wet_amount += wet_rate * Time.deltaTime;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (1 - wet_amount));
            if (wet_amount > wet_limit)
            {
                GameHandler.instance.playerHealth.LoseHealth(1);
            }

        }
    }

}
