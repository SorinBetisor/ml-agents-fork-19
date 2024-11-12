using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public class PlayerHearing : MonoBehaviour, ISensor
{
    private Vector3 lastHeardPosition;
    private bool heardSound;
    private string sensorName = "PlayerHearing";
    private ObservationSpec observationSpec;

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

    public string GetName() {
        return sensorName;
    }

    public int Write(ObservationWriter writer) {
        writer.Add(lastHeardPosition);
        return 1; // Number of elements written (x, y, z)
    }

    public byte[] GetCompressedObservation() {
        return null;
    }

    public void Update() {
        // We do not need anything
    }

    public void Reset() {
        lastHeardPosition = new Vector3(0f, 0f, 0f);
    }

    public CompressionSpec GetCompressionSpec() {
        return CompressionSpec.Default();
    }

    public ObservationSpec GetObservationSpec() {
        return observationSpec;
    }
}
