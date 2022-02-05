using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralaxing : MonoBehaviour
{
    public float paralaxSpeed;
    public Material myMat;
    void Start()
    {
        //myMat = transform.GetComponent<Material>();
    }

    // Update is called once per frame
    void Update()
    {
        myMat.mainTextureOffset += new Vector2(paralaxSpeed*Time.deltaTime, 0);
    }
}
