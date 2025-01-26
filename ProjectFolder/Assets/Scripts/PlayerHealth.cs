using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int health = 5;
    //bool damaged;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private Animator[] animators;
    [SerializeField] public float[] resetPosX;
    [SerializeField] public float[] heartFallTimer;
    [SerializeField] public bool[] damaged;
    float resetPosY = 188.4f;
    float addHealthTimer = 1.2f;

    bool addHealthBool = false;
    int addHealths = 0;

    float death_anim_timer = 2.5f;
    WaitForSeconds death_anim_delay = new WaitForSeconds(3.0f);

    public GameObject gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        ChangeHearts();
        health = 5;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.O))
        //{
        //    LoseHealth(1);
        //}
        //else if (Input.GetKeyUp(KeyCode.I))
        //{
        //    AddHealth(1);
        //}
        DamagedAnim();


        if (addHealthBool)
        {
            addHealthTimer -= Time.deltaTime;
            if (addHealthTimer < 0)
            {
                health += addHealths;
                ChangeHearts();
                addHealthTimer = 1.2f;
                addHealthBool = false;
            }
        }

        if (health <= 0)
        {
            GameHandler.instance.playerMovement.stopMoving();
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void LoseHealth(int damage)
    {
        health -= damage;
        if (health > 0)
        {
            GameHandler.instance.sfxScript.PlayPlayerLoseHP();
        }
        else if (health <= 0)
        {
            GameHandler.instance.sfxScript.PlayPlayerDie();
            gameOverScreen.SetActive(true);
            GameStateManager.IN_CUTSCENE = true;
        }

        SetDamaged();
        ChangeHearts();
        PlayDeathAnim();
        GameHandler.instance.playerMovement.CloseInven();
    }

    public void AddHealth(int addHealth)
    {
        addHealthBool = true;
        addHealths = addHealth;
    }


    void ChangeHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].transform.localPosition = new Vector3((float)resetPosX[i], resetPosY, 0);
                hearts[i].transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                hearts[i].SetActive(true);
            }
        }
    }

    void DamagedAnim()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
            }
            else
            {
                if (damaged[i])
                {
                    if (hearts[i].activeInHierarchy)
                    {
                        heartFallTimer[i] -= Time.deltaTime;
                        animators[i].SetTrigger("Die");
                        if (heartFallTimer[i] < 0)
                        {
                            hearts[i].SetActive(false);
                            heartFallTimer[i] = 1.2f;
                            damaged[i] = false;
                        }
                    }
                }
            }
        }
    }

    void SetDamaged()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
            }
            else
            {
                damaged[i] = true;
            }
        }
    }

    void PlayDeathAnim() //CALLED IN ONE FRAME ONCE UPON START DEATH - (uSE cOROUTINE like i did for lights if u wan continuous update over the anim)
    {
        GameStateManager.IN_CUTSCENE = true;
        StartCoroutine(StartDeathAnim());
        GameHandler.instance.lightsManager.StartDarkenScene(death_anim_timer);
    }
    void EndDeathAnim() //CALLED IN ONE FRAME ONCE DEATH ANIM FINISHES PLAYING
    {
        GameStateManager.IN_CUTSCENE = false;
        // Reset Affected Gameplay Vars
        GameHandler.instance.lightsManager.ResetSceneLight();
        GameHandler.instance.playerStatus.ResetPlayerStatus();
        // Reset Player Position to Last Checkpoint
        Vector2 respawnPos = GameHandler.instance.checkpointManager.GetLastSpawnPoint();
        GameHandler.instance.player.GetComponent<Rigidbody2D>().MovePosition(respawnPos);
        Debug.LogWarning("Player Should Be Moved to: " + respawnPos);
    }

    IEnumerator StartDeathAnim() //JUST TO WAIT NUM OF SECONDS OF DEATH ANIM BEFORE FINISH
    {
        yield return death_anim_delay;
        EndDeathAnim();
    }

}
