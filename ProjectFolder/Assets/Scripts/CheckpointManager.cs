using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public List<Checkpoint> unlocked_checkpoints = new List<Checkpoint>();

    public void AddCheckpoint(Checkpoint checkpoint)
    {
        unlocked_checkpoints.Add(checkpoint);
    }

    public Vector2 GetLastSpawnPoint()
    {
        Checkpoint checkpoint = null;
        foreach (Checkpoint cp in unlocked_checkpoints)
        {
            if (checkpoint == null)
            {
                checkpoint = cp;
            }
            else if (checkpoint.ID < cp.ID)
            {
                checkpoint = cp;
            }
        }

        if (checkpoint != null)
            return checkpoint.SpawnPos;
        else
            return GameHandler.instance.playerSpawnPoint;

    }
   
}
