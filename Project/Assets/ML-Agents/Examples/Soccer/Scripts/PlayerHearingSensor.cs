using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using System;


public class PlayerHearingSensor : ISensor
{
    [System.Flags]
    public enum Model
    {
        None = 0,
        Relative = 1,
        Normalized = 2,
        Directed = 4,
        Continuous = 8
    }
    
    private Vector3 lastHeardPosition;
    private Vector3 relativePosition;
    private string sensorName = "PlayerHearingSensor";
    private ObservationSpec observationSpec;
    GameObject AgentObject;
    const float minValueZ = -7.6f;
    const float minValueX = -17.5f;
    const float maxValueZ = 7.6f;
    const float maxValueX = 17.5f;
    private Model features;

    public PlayerHearingSensor(GameObject agentObject, Model model) {
        observationSpec = ObservationSpec.Vector(3);
        AgentObject = agentObject;
        features = model;
    }

    // Called when the player hears a sound
    public void ReceiveSignal(Vector3 ballPosition)
    {
        lastHeardPosition = ballPosition;

        if (((int) features & (int) Model.Continuous) == 0)
            ApplyFeatures();
    }

    public Vector3 ApplyRelative(Vector3 ballPosition)
    {
        return new Vector3(
            ballPosition.x - AgentObject.transform.position.x,
            ballPosition.y,
            ballPosition.z - AgentObject.transform.position.z
        );
    }

    public Vector3 ApplyNormalization(Vector3 ballPosition)
    {
        return new Vector3(
            (ballPosition.x - minValueX)/(maxValueX - minValueX),
             ballPosition.y,
            (ballPosition.z - minValueZ)/(maxValueZ - minValueZ)
        );
    }

    public Vector3 ApplyDirection(Vector3 ballPosition)
    {
        return ApplyRotation(ballPosition, AgentObject.transform.rotation.y);
    }

    public Vector3 ApplyRotation(Vector3 position, float degrees) {
        float angle = (float) (degrees * Math.PI) / 180f; // convert degrees to radians
        
        return new Vector3(
            (float) ( position.x * Math.Cos(angle) + position.z * Math.Sin(angle)),
                      position.y,
            (float) (-position.x * Math.Sin(angle) + position.z * Math.Cos(angle))
        );
    }

    public string GetName() {
        return sensorName;
    }

    public int Write(ObservationWriter writer) {
        writer.Add(relativePosition);
        return 3; // Number of elements written (x, y, z)
    }

    public byte[] GetCompressedObservation() {
        return null;
    }

    public void Update() {
        if (((int) features & (int) Model.Continuous) == 1)
            ApplyFeatures();
    }

    private void ApplyFeatures()
    {
        relativePosition = lastHeardPosition;

        if (((int) features & (int) Model.Relative) == 1)
            relativePosition = ApplyRelative(relativePosition);
        
        if (((int) features & (int) Model.Normalized) == 1)
            relativePosition = ApplyNormalization(relativePosition);
        
        if (((int) features & (int) Model.Directed) == 1)
            relativePosition = ApplyDirection(relativePosition);
    }

    public void Reset() {
        lastHeardPosition = new Vector3(0f, 0f, 0f);
        relativePosition = new Vector3(0f, 0f, 0f);
    }

    public CompressionSpec GetCompressionSpec() {
        return CompressionSpec.Default();
    }

    public ObservationSpec GetObservationSpec() {
        return observationSpec;
    }
}
