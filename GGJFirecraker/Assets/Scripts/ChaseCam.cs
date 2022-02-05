using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCam : MonoBehaviour
{
    private Transform myTrans;
    private Transform player;
    private Vector3 targetPos;
    void Start()
    {
        myTrans = transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = myTrans.position;
        targetPos.x = player.position.x;
        myTrans.position = targetPos;

    }
}
