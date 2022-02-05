using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Transform collectableToDisplay;
    private Transform myplayer;
    private Player playerScript;
    private GameObject myPicture;
    private bool hasShown = false;
    private Vector3 collectableSpawnPoint;
    private float collectableZ =-2.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hasShown&&Input.GetMouseButtonUp(0))
        {
           playerScript.enabled = true;
           Destroy(myPicture);
           Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.transform.tag.Equals("Player")&&Input.GetKey(KeyCode.E)&&!hasShown)
        {
            myplayer = collision.transform;
            playerScript = myplayer.GetComponent<Player>();
            playerScript.enabled = false;
            collectableSpawnPoint = transform.position;
            collectableSpawnPoint.z = collectableZ;
            myPicture = (GameObject)Instantiate(collectableToDisplay.gameObject, collectableSpawnPoint, collectableToDisplay.transform.rotation);
            hasShown = true;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("Player") && Input.GetKey(KeyCode.E)&&!hasShown)
        {
            myplayer = collision.transform;
            playerScript = myplayer.GetComponent<Player>();
            playerScript.enabled = false;
            collectableSpawnPoint = transform.position;
            collectableSpawnPoint.z = collectableZ;
            myPicture = (GameObject)Instantiate(collectableToDisplay.gameObject, collectableSpawnPoint, collectableToDisplay.transform.rotation);
            hasShown = true;
        }

    }
}
