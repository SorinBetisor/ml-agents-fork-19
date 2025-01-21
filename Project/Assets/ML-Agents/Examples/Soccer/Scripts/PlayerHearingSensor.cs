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
    public enum Model
    {
        Coordinates,
        RelativeNormalization,
        RelativeRotationNormalization,
        RelativeRotation
    }
    
    private Vector3 lastHeardPosition;
    private Vector3 relativePosition;
    private string sensorName = "PlayerHearingSensor";
    private ObservationSpec observationSpec;
    GameObject AgentObject;
    float minValueZ = -7.6f;
    float minValueX = -17.5f;
    float maxValueZ = 7.6f;
    float maxValueX = 17.5f;
    private Model m;

    public PlayerHearingSensor(GameObject agentObject, Model model) {
        observationSpec = ObservationSpec.Vector(3);
        AgentObject = agentObject;
        m = model;
    }

    // Called when the player hears a sound
    public void ReceiveSignal(Vector3 ballPosition)
    {
        lastHeardPosition = ballPosition;
    }

    public Vector3 ApplyCoordinates(Vector3 ballPosition)
    {
        return ballPosition;
    }
    public Vector3 ApplyRelativeNormalization(Vector3 ballPosition)
    {
        ballPosition.x = ballPosition.x - AgentObject.transform.position.x;
        ballPosition.z = ballPosition.z - AgentObject.transform.position.z;
        float x = (ballPosition.x - minValueX)/(maxValueX - minValueX);
        float z = (ballPosition.z - minValueZ)/(maxValueZ - minValueZ);
        Vector3 normalized = new Vector3(x, ballPosition.y, z);
        return normalized;
    }

    public Vector3 ApplyRelativeRotationNormalization(Vector3 ballPosition)
    {
        ballPosition.x = ballPosition.x - AgentObject.transform.position.x;
        ballPosition.z = ballPosition.z - AgentObject.transform.position.z;
        float x = (ballPosition.x - minValueX)/(maxValueX - minValueX);
        float z = (ballPosition.z - minValueZ)/(maxValueZ - minValueZ);
        Vector3 normalized = new Vector3(x, ballPosition.y, z);
        Vector3 afterRotation = ApplyRotation(normalized, AgentObject.transform.rotation.y);
        return afterRotation;
    }

    public Vector3 ApplyRelativeRotation(Vector3 ballPosition)
    {
        ballPosition.x = ballPosition.x - AgentObject.transform.position.x;
        ballPosition.z = ballPosition.z - AgentObject.transform.position.z;
        Vector3 normalized = new Vector3(ballPosition.x, ballPosition.y, ballPosition.z);
        Vector3 afterRotation = ApplyRotation(normalized, AgentObject.transform.rotation.y);
        return afterRotation;
    }

    public Vector3 ApplyRotation(Vector3 position, float degrees) {
        float angle = (float) (degrees * Math.PI)/180f; // convert degrees to radians
        
        float rotated_X = (float) (position.x * Math.Cos(angle) + position.z * Math.Sin(angle));
        float rotated_Z = (float) (-position.x * Math.Sin(angle) + position.z * Math.Cos(angle));
        Vector3 newPosition = new Vector3(rotated_X, position.y, rotated_Z);
        
        return newPosition;
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
        switch(m)
        {
            case Model.Coordinates:
                relativePosition = ApplyCoordinates(ballPosition);
                break;
            case Model.RelativeNormalization:
                relativePosition = ApplyRelativeNormalization(ballPosition);
                break;
            case Model.RelativeRotationNormalization:
                relativePosition = ApplyRelativeRotationNormalization(ballPosition);
                break;
            case Model.RelativeRotation:
                relativePosition = ApplyRelativeRotation(ballPosition);
                break;
        }
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
