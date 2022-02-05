using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public Transform obstacleToShoot;
    public Transform[] shootpoints;
    private float fireRate = 2.0f;
    private float timeSinceFire = 0.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceFire >= fireRate)
        {
            shootObstacles();
            timeSinceFire = 0.0f;
        }
        else
        {
            timeSinceFire += Time.deltaTime;
        }
    }

    private void shootObstacles()
    {
        for (int i=0;i<shootpoints.Length;i++)
        {
            Instantiate(obstacleToShoot, shootpoints[i].position, obstacleToShoot.rotation);
        }
    }
}
