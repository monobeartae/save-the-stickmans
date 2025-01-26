using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    Transform target;
    Vector3 targetPos;
    public Vector2 minPos;
    public Vector2 maxPos;
    public float smooth;

    // Start is called before the first frame update
    void Start()
    {
        target = GameHandler.instance.player.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position != target.position)
        {
            targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

            //clamp camera to not move out of map
            targetPos.x = Mathf.Clamp(target.position.x, minPos.x, maxPos.x);
            targetPos.y = Mathf.Clamp(target.position.y, minPos.y, maxPos.y);

            transform.position = Vector3.Lerp(transform.position, targetPos, smooth);
        }
    }
}
