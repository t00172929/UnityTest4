using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCam : MonoBehaviour
{
    private Transform player;
    private Transform myTrans;
    private Vector3 playerCamVect;
    private const float yOffset = 2.99f;
    private const float yThreshold = 4.5f;
    private const float  negThreshold = -3.0f;
    // when cam reaces Y threshold move cam vertical then
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        myTrans = gameObject.transform;
    }


    void Update()
    {
        playerCamVect = player.position;
        playerCamVect.z = myTrans.position.z;
        if (player.position.y - myTrans.position.y >= yThreshold)
        {
            playerCamVect.y += yOffset;
        }
        else if (player.position.y - myTrans.position.y <= negThreshold)
        {
            playerCamVect.y += yOffset;
        }
        else
        {
            playerCamVect.y = myTrans.position.y;
        }
        myTrans.position = playerCamVect;
    }
}
