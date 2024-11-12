using System.Collections.Generic;
using UnityEngine;

public class BallSignal : MonoBehaviour
{
    private List<HearingAgent> nearbyAgents = new List<HearingAgent>();

    private void OnTriggerEnter(Collider other) 
    {
        HearingAgent agent = other.GetComponent<HearingAgent>();
        if (agent != null) 
        {
            nearbyAgents.Add(agent);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        HearingAgent agent = other.GetComponent<HearingAgent>();
        if (agent != null) 
        {
            nearbyAgents.Remove(agent);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var agent in nearbyAgents)
        {
            PlayerHearing hearing = agent.GetComponent<PlayerHearing>();
            if (hearing != null)
            {
                hearing.ReceiveSignal(transform.position);
            }
        }
    }
}
