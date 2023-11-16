using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerPos;
    // Update is called once per frame
    void Update()
    {
        transform.position = playerPos.position;
    }
}
