using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeWall : MonoBehaviour
{
    public float moveSpeed;
    private Transform myTrans;
    private Vector2 targetPos;
    void Start()
    {
        myTrans = transform;
        targetPos = new Vector2(myTrans.position.x + 1000, myTrans.position.y);
        
    }

    // Update is called once per frame
    void Update()
    {
        myTrans.position = Vector2.MoveTowards(myTrans.position, targetPos, moveSpeed * Time.deltaTime);
    }
}
