using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class PlayerHearing : MonoBehaviour, ISensor
{
    private Vector3 lastHeardPosition;
    private bool heardSound;

    // Called when the player hears a sound
    public void ReceiveSignal(Vector3 ballPosition)
    {
        lastHeardPosition = ballPosition;
        heardSound = true;
        Debug.Log(gameObject.name + " heard the ball collision at " + ballPosition);
    }

    // Method to get the last heard position and reset the heard state
    public bool GetHeardStatus(out Vector3 position)
    {
        position = lastHeardPosition;
        bool heard = heardSound;
        heardSound = false; // Reset the state after checking
        return heard;
    }
}
