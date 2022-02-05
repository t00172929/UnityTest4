using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Things to do
 *  things done
 *  - movement
 *  - double jump
 *  - jump dash
 *  - telport
 *  - essence pickup
 *  - rope
 *  - moveing platforms
 *  - cursed pull
 *
 * 
 * */
public class Player : MonoBehaviour
{
    public AudioSource jumpFX;
    public AudioSource landFX;
    private LevelManager mylm;
    private Animator myAnim;
    private Camera myCam;
    private SpriteRenderer mySprite;
    private Light myLight;
    private CircleCollider2D teleportUICol;
    private Transform myTrans;
    private Transform teleportUI;
    private Rigidbody2D myRigid;
    private Vector2 moveVector;
    private Vector2 resistenceVector;
    private Vector2 teleportPosition;
    private Vector2 cursePullDir;
    private float slopeHeight = 0.8f;
    private float maxMagnitude = 6.0f;
    private float jumpForce = 9.0f;
    private float dashForce = 10.0f;
    private float teleportDist = 4.7f;
    private const float circleCastRadius = 0.2f;
    private const float circleCastDistance = 2.0f;//1.0f;
    private float maxSpeed;
    private float MAX_WALK_SPEED = 600.0F;
    private const float MAX_SPRINT_SPEED = 1.0F;
    private float inputY;
    private float inputX;
    private float ropeFlipTime=0.0f;
    private float maxRopeFlipTime = 0.3f;
    private int essenceCollected = 1;//2;
    private bool hasDoubleJumped = false;
    private bool hasAirDashed = false;
    private bool isOnRope;
    private bool isOnSlope;
    public bool isASprite;

    void Start()
    {
        myTrans = transform.GetComponent<Transform>();
        myRigid = myTrans.GetComponent<Rigidbody2D>();
        myCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        teleportUI = myTrans.GetChild(0);
        teleportUICol = teleportUI.GetComponent<CircleCollider2D>();
        myLight = myTrans.GetChild(2).GetComponent<Light>();
        myAnim = myTrans.GetChild(1).GetComponent<Animator>();
        mylm = LevelManager.Instance;
        if(isASprite)
        {
            mySprite = myTrans.GetChild(1).GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!teleportUI.gameObject.activeSelf)
        {
            move();
            jump();
            airDash();
        }
        teleport();
        inputX = Input.GetAxis("Horizontal");
        scaleTeleportUItoDist();
        if(mySprite)
        {
            handleSpriteRotation();
        }
        // Debug.Log("UI bounds are " + teleportUI.GetComponent<CircleCollider2D>().bounds);
     
       
    }
    private void move()
    {
        walkanimation();
        //Debug.Log("inputx is " + inputX);
        
        if (!isOnRope&&!isOnSlope)
        {
            moveVector.x = inputX;
            if (myRigid.velocity.magnitude > maxMagnitude)
            {
                //myRigid.AddForce(moveVector * MAX_WALK_SPEED * Time.deltaTime);
            }
            else
            {
                myRigid.AddForce(moveVector * MAX_WALK_SPEED * Time.deltaTime);

            }
        }
        else if(isOnRope)
        {
            inputY = Input.GetAxis("Vertical");
            moveVector.y = inputY;
            if(inputY!=0)
            {
                myRigid.AddForce(moveVector * MAX_WALK_SPEED/4 * Time.deltaTime);
                ropeFlipTime += Time.deltaTime;
                if(ropeFlipTime>=maxRopeFlipTime)
                {
                    mySprite.flipX = !mySprite.flipX;
                    ropeFlipTime = 0.0f;
                }
            }
            
        }
        else if(isOnSlope)
        {
            //moveVector.y = 0.8f;
            moveVector.x = inputX;
            //Debug.Log("Move vector is " + moveVector+ "input x is "+inputX);
            if (inputX<0)
            {
                moveVector.y = slopeHeight;
            }
            else if(inputX>0)
            {
                moveVector.y = slopeHeight;
            }
            else
            {
                moveVector.y = 0.0f;
            }
            myRigid.AddForce(moveVector * MAX_WALK_SPEED * Time.deltaTime);
                
        }
        //Debug.Log("Magbnitude is " + myRigid.velocity.magnitude.ToString());
        if(isOnRope&&Input.GetKeyUp(KeyCode.LeftArrow)&&myTrans.localPosition.x>0.0f)
        {
            myTrans.localPosition = new Vector2(-myTrans.localPosition.x, myTrans.localPosition.y);
            mySprite.flipX = true;
        }
        if (isOnRope && Input.GetKeyUp(KeyCode.RightArrow) && myTrans.localPosition.x < 0.0f)
        {
            myTrans.localPosition = new Vector2(-myTrans.localPosition.x, myTrans.localPosition.y);
            mySprite.flipX = false;
        }
    }
    private void jump()
    {
        if (isOnRope)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                moveVector.x = inputX;
                myRigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                myRigid.AddForce(moveVector * MAX_WALK_SPEED, ForceMode2D.Force);
                myAnim.SetBool("ShouldJump", true);
                jumpFX.Play();
                myAnim.SetBool("ShouldClimb", false);
            }

        }
        else
        {


            if (Input.GetKeyUp(KeyCode.Space) && isGrounded())
            {
                myRigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                myAnim.SetBool("ShouldJump", true);
                myAnim.SetBool("ShouldLand", false);
                jumpFX.Play();
            }
            else if (Input.GetKeyUp(KeyCode.Space) && !isGrounded() && !hasDoubleJumped)
            {
                myRigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                hasDoubleJumped = true;
                myAnim.SetBool("ShouldJump", true);
                myAnim.SetBool("ShouldLand", false);
                myAnim.Play("2DJump",0);
                jumpFX.Play();
                //reset animation
            }
            if (hasDoubleJumped && isGrounded())
            {
                hasDoubleJumped = false;
            }
        }
    }
    private bool isGrounded()
    {
        RaycastHit2D myHit = Physics2D.CircleCast(myTrans.position, circleCastRadius, Vector2.down, circleCastDistance);
        if (myHit)
        {
            // Debug.Log("hit "+myHit.transform.name);
            if (myAnim.GetBool("ShouldJump")&& myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
            {
                myAnim.SetBool("ShouldLand", true);
                landFX.Play();
                myAnim.SetBool("ShouldJump", false);

            }
            return true;
        }
        else
        {
            return false;
        }
    }
    private void airDash()
    {
        if (!hasAirDashed)
        {
            // air dash Right
            if (inputX > 0.1f && Input.GetKeyDown(KeyCode.Z) && !isGrounded())
            {
                myRigid.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse);
                hasAirDashed = true;
            }
            //air dash left
            else if (inputX <= -0.1f && Input.GetKeyDown(KeyCode.Z) && !isGrounded())
            {
                myRigid.AddForce(Vector2.left * dashForce, ForceMode2D.Impulse);
                hasAirDashed = true;
            }
        }
        else if (hasAirDashed && isGrounded())
        {
            hasAirDashed = false;
        }
    }
    private void teleport()
    {
        if (isGrounded() && Input.GetKey(KeyCode.X))
        {

            enableTeleportUI();
            myAnim.SetBool("ShouldSlash", true);
            //enable UI
            if (Input.GetMouseButtonUp(0))
            {
                teleportPosition = Input.mousePosition;
                teleportPosition = (Vector2)myCam.ScreenToWorldPoint(teleportPosition);
                if(Vector2.Distance(teleportPosition,myTrans.position)<= teleportDist)
                {
                    myTrans.position = teleportPosition;
                }
            }
        }
        if(Input.GetKeyUp(KeyCode.X))
        {
            disableTeleportUI();
            myAnim.SetBool("ShouldSlash", false);
        }
    }
    private void scaleTeleportUItoDist()
    {
        if(teleportUICol.bounds.extents.y<teleportDist&&teleportUI.gameObject.activeSelf)
        {
            teleportUI.localScale += Vector3.one;
        }
    }
    private void enableTeleportUI()
    {
        teleportUI.gameObject.SetActive(true);
    }
    private void disableTeleportUI()
    {
        teleportUI.gameObject.SetActive(false);
    }
    private void handleSpriteRotation()
    {
        if (!isOnRope)
        {
            if (inputX > 0 && !mySprite.flipX)
            {
                mySprite.flipX = true;
            }
            if (inputX < 0 && mySprite.flipX)
            {
                mySprite.flipX = false;
            }
        }
    }
    private void walkanimation()
    {
        if (isGrounded() && !isOnRope)
        {
            if (inputX > 0.4f || inputX < -0.4f)
            {
                myAnim.SetBool("ShouldRun", true);
            }
            else
            {
                myAnim.SetBool("ShouldRun", false);
            }
        }
        else
        {
            myAnim.SetBool("ShouldRun", false);
        }
    }
    public int getCollectedEssence()
    {
        return essenceCollected;
    }
    public void takeDamage()
    {

    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
       // Debug.Log("Colliding with " + collision.gameObject.name);
        if(collision.transform.tag.Equals("MovingPlatform"))
        {
            myTrans.parent = collision.transform;
        }
        else if(collision.transform.tag.Equals("Rope"))
        {
            myTrans.parent = collision.transform;
            myRigid.gravityScale = 0;
            isOnRope = true;
            moveVector.x = 0;
            myAnim.SetBool("ShouldClimb", true);
        }
        else if(collision.transform.tag.Equals("Slope"))
        {
            isOnSlope = true;

        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.transform.tag.Equals("MovingPlatform"))
        {
            myTrans.parent = null;
        }
        if (collision.transform.tag.Equals("Rope"))
        {
            //Debug.LogError("ropeOff");
             myTrans.parent = null;
             myRigid.gravityScale = 1;
             isOnRope = false;
        }
        else if (collision.transform.tag.Equals("Slope"))
        {
            isOnSlope = false;
            moveVector.y = 0;

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Essence"))
        {
            Destroy(collision.gameObject);
            essenceCollected++;
            Debug.Log("Collected essence, amount collected is " + essenceCollected.ToString());
            teleportDist += 0.5f;
            maxSpeed += 100.0f;
            dashForce += 1.0f;
            myLight.range -= 10.0f;
            maxMagnitude += 0.3f;
        }
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("CursePull"))
        {
           // Debug.Log("Pull player distance between  trigger and player is "+ Vector2.Distance(collision.transform.position, myTrans.position));
            cursePullDir = (collision.transform.position - myTrans.position).normalized;
            myRigid.AddForce(cursePullDir * MAX_WALK_SPEED/2 * essenceCollected * Time.deltaTime, ForceMode2D.Force);
            if(Vector2.Distance(collision.transform.position, myTrans.position) < 0.2f)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
