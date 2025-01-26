using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXScript : MonoBehaviour
{
    public AudioSource BuyItem;
    public AudioSource InventoryOpen;
    public AudioSource PickupItem;
    public AudioSource PlayerLoseHP;
    public AudioSource UnableToBuy;
    public AudioSource Gunshot;
    public AudioSource EraserThrow;
    public AudioSource WaterSplash;
    public AudioSource EnemyDie;
    public AudioSource PlayerDie;
    public AudioSource Checkpoint;

    public void PlayBuyItem()
    {
        BuyItem.Play();
    }
    public void PlayInventoryOpen()
    {
        InventoryOpen.Play();
    }
    public void PlayPickupItem()
    {
        PickupItem.Play();
    }
    public void PlayPlayerLoseHP()
    {
        PlayerLoseHP.Play();
    }
    public void PlayUnableToBuy()
    {
        UnableToBuy.Play();
    }
    public void PlayGunshot()
    {
        Gunshot.Play();
    }
    public void PlayEraser()
    {
        EraserThrow.Play();
    }
    public void PlayWaterSplash()
    {
        WaterSplash.Play();
    }
    public void PlayEnemyDie()
    {
        EnemyDie.Play();
    }
    public void PlayPlayerDie()
    {
        PlayerDie.Play();
    }
    public void PlayCheckpoint()
    {
        Checkpoint.Play();
    }
}
