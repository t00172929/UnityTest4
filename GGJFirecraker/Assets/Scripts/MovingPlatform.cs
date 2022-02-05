using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float horizontalDistanceToTravel;
    public float verticalDistanceToTravel;
    public float moveSpeed;
    private const float maxDistToGoal = 0.2f;
    private Vector2 travelVector;
    private Vector2 startPos;
    private Vector2 dirVector;
    private Transform myTrans;
    private Rigidbody2D myRigid;
    private bool shouldMoveBackToStart = false;
    void Start()
    {
        myTrans = gameObject.transform;
        startPos = (Vector2)myTrans.position;
        travelVector = new Vector2(horizontalDistanceToTravel+startPos.x, verticalDistanceToTravel+startPos.y);
        dirVector = new Vector2(horizontalDistanceToTravel, verticalDistanceToTravel).normalized;
        if(horizontalDistanceToTravel ==0.0f)
        {
            dirVector.x = 0.0f;
        }
        else if(verticalDistanceToTravel ==0.0f)
        {
            dirVector.y = 0.0f;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        moveToGoal();
    }
    private void moveToGoal()
    {
        
        if(Vector2.Distance(travelVector,myTrans.position)<maxDistToGoal&&!shouldMoveBackToStart)
        {
            shouldMoveBackToStart = true;
        }
        else if(Vector2.Distance(startPos, myTrans.position) < maxDistToGoal && shouldMoveBackToStart)
        {
            shouldMoveBackToStart = false;
        }
        if(shouldMoveBackToStart)
        {
            //myRigid.velocity = -dirVector * moveSpeed * Time.deltaTime;
            myTrans.position = Vector2.MoveTowards(myTrans.position, startPos,moveSpeed*Time.deltaTime);
        }
        else
        {
            //myRigid.velocity = dirVector * moveSpeed * Time.deltaTime;
            myTrans.position = Vector2.MoveTowards(myTrans.position, travelVector,moveSpeed*Time.deltaTime);
        }
    }
}
