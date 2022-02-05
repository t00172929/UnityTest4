using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Crystal : MonoBehaviour
{
    public float moveSpeed;
    public float horizontalDistanceToTravel;
    public float verticalDistanceToTravel;
    public bool isEndCrystal;
    public int nextLevel;
    private Vector2 travelVector;
    private Vector2 startPos;
    private Transform myTrans;
    private bool shouldMoveBackToStart;
    private float maxDistToGoal = 0.1f;
    private LevelManager mylm;

    void Start()
    {
        myTrans = gameObject.transform;
        startPos = (Vector2)myTrans.position;
        travelVector = new Vector2(horizontalDistanceToTravel + startPos.x, verticalDistanceToTravel + startPos.y);
        mylm = LevelManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        moveToGoal();
    }
    private void moveToGoal()
    {

        if (Vector2.Distance(travelVector, myTrans.position) < maxDistToGoal && !shouldMoveBackToStart)
        {
            shouldMoveBackToStart = true;
        }
        else if (Vector2.Distance(startPos, myTrans.position) < maxDistToGoal && shouldMoveBackToStart)
        {
            shouldMoveBackToStart = false;
        }
        if (shouldMoveBackToStart)
        {
            //myRigid.velocity = -dirVector * moveSpeed * Time.deltaTime;
            myTrans.position = Vector2.MoveTowards(myTrans.position, startPos, moveSpeed * Time.deltaTime);
        }
        else
        {
            //myRigid.velocity = dirVector * moveSpeed * Time.deltaTime;
            myTrans.position = Vector2.MoveTowards(myTrans.position, travelVector, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag.Equals("Player")&&isEndCrystal)
        {
            mylm.incEssence(collision.transform.GetComponent<Player>().getCollectedEssence());
            SceneManager.LoadScene(nextLevel);
        }
    }
}
