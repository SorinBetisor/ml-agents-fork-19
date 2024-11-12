using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallColision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "purpleAgent" || col.gameObject.tag == "blueAgent" || col.gameObject.tag == "wall"){
            Debug.Log("sound" + transform.position);

        }
    }
}
