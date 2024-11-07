using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSignal : MonoBehaviour
{
    private List<PlayerHearing> nearbyPlayers = new List<PlayerHearing>();

    private void OnTriggerEnter(Collider other) 
    {
        PlayerHearing player = other.GetComponent<PlayerHearing>();
        if (player != null) 
        {
            nearbyPlayers.Add(player);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        PlayerHearing player = other.GetComponent<PlayerHearing>();
        if (player != null) {
            nearbyPlayers.Remove(player);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach(var player in nearbyPlayers)
        {
            player.ReceiveSignal(transform.position);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
