using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacles : MonoBehaviour
{
    public bool shouldFallDown;
    private Transform myTrans;
    private float speed = 10f;
    private Vector2 moveDir;
    private float lifeTimer = 0.0f;
    private float maxLifeTime = 5.0f;
    private float rotAmt;
    private Vector3 rotVec;
    private float rotSpeed = 0.8f;
    void Start()
    {
        myTrans = gameObject.transform;
        moveDir = myTrans.position;
        if (shouldFallDown)
        {
            moveDir.y -= 50.0f;
        }
        else
        {
            moveDir.y += 50.0f;
        }
        rotVec = Vector3.zero;
        rotAmt = Random.Range(0, 5f);
        rotVec.z = rotAmt;
    }

    // Update is called once per frame
    void Update()
    {
        
        myTrans.position = Vector2.MoveTowards(myTrans.position, moveDir,speed*Time.deltaTime);
        if(shouldFallDown)
        myTrans.Rotate(rotVec * rotSpeed);

        if(lifeTimer>=maxLifeTime)
        {
            Destroy(this.gameObject);
        }
        lifeTimer += Time.deltaTime;
    }
}
