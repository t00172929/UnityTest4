using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneTransition : MonoBehaviour
{
    private const float maxCutsceneTime = 5.0f;
    private float custsceneTime = 0.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        custsceneTime += Time.deltaTime;
        if(custsceneTime>=maxCutsceneTime)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
