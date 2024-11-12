using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallColision : MonoBehaviour
{

    public GameObject area;
    [HideInInspector]
    public SoccerEnvController envController;

    // Start is called before the first frame update
    void Start()
    {
         envController = area.GetComponent<SoccerEnvController>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "purpleAgent" || col.gameObject.tag == "blueAgent" || col.gameObject.tag == "wall"){
            //Debug.Log("sound" + transform.position);
            envController.NotifyPlayersHearing(transform.position);

        }
    }
}
