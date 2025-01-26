using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//gets the player for the current scene and sets it
//to use code in scripts:
//1) make variable: Transform target;
//2) call in the start function: target = GameHandler.instance.player.transform;
//if still don't understand, can refer to the Minimap.cs script
//now don't need to keep creating public player objects in the script and keep having to drag and drop the player constantly KSDNFJKSNGSJKN

public class GameHandler : MonoBehaviour
{
    #region Singleton

    public static GameHandler instance;
    void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject player;
    public Vector2 playerSpawnPoint;
    public PlayerHealth playerHealth;
    public PlayerMovement playerMovement;
    public PlayerStatus playerStatus;
    public PlayerWeapon playerWeapon;
    public PlayerInventory playerInventory;
    public InventoryUI inventoryUI;
    public SceneLightsManager lightsManager;
    public CheckpointManager checkpointManager;
    public PopUpManager popupManager;
    public Tilemap tilemapWet;
    public LevelLoader levelLoader;
    public SFXScript sfxScript;


    void Update()
    {
        IceCube.DryWetTiles();
    }

}
