using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    float timer;
    LevelLoader levelLoader;

    private void Start()
    {
        timer = 29f;
        levelLoader = GameHandler.instance.levelLoader;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            levelLoader.LoadNextLevel("MainMenuScene");
        }
    }
}
