using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class KillObject : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.transform.name);
        if (other.tag.Equals("Player"))
        {
            // restart level when closer to completion
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
          //  Destroy(other.gameObject);
        }
    }
}
