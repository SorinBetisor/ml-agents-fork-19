using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using System.Collections.Generic;

public class MemorySensorA : MonoBehaviour, ISensor
{
    private Transform agentTransform;
    private Rigidbody agentRigidbody;
    private GameObject ball;
    private Rigidbody ballRigidbody;
    public int observationSize = 15;
    private Queue<float[]> memoryQueue;
    public int memorySize = 5;
    
    private string sensorName = "MemorySensorA";
    private ObservationSpec observationSpec;

    void Awake()
    {
        // Initialize queue immediately when the object is created
        memoryQueue = new Queue<float[]>();
        
        // Initialize other components
        agentTransform = GetComponent<Transform>();
        agentRigidbody = GetComponent<Rigidbody>();
        
        // Initialize observation spec
        observationSpec = ObservationSpec.Vector(observationSize * memorySize);
    }
    
    void Start()
    {
        // Find ball after all objects are initialized
        ball = GameObject.FindGameObjectWithTag("ball");
        if (ball != null)
        {
            ballRigidbody = ball.GetComponent<Rigidbody>();
            Debug.Log("Ball found");
        }
        else
        {
            Debug.LogError("Ball not found! Make sure there's a GameObject with 'Ball' tag in the scene.");
        }
    }

    private void UpdateMemory(float[] observation)
    {
        memoryQueue.Enqueue(observation);
        if (memoryQueue.Count > memorySize)
        {
            memoryQueue.Dequeue();
        }
    }

    public CompressionSpec GetCompressionSpec()
{
        return CompressionSpec.Default();
    }

    // ISensor implementation
    public string GetName()
    {
        return sensorName;
    }

    public ObservationSpec GetObservationSpec()
    {
        return observationSpec;
    }

    public int Write(ObservationWriter writer)
    {
        var index = 0;
        foreach (var observation in memoryQueue)
        {
            foreach (var value in observation)
            {
                writer[index] = value;
                index++;
            }
        }
        return index;
    }

    public byte[] GetCompressedObservation()
    {
        return null;
    }

    public void Update()
    {
        float[] currentObservation = new float[observationSize];
        int index = 0;

        // Agent's own observations
        Vector3 agentPosition = agentTransform.position;
        currentObservation[index++] = agentPosition.x;
        currentObservation[index++] = agentPosition.y;
        currentObservation[index++] = agentPosition.z;
        
        Vector3 agentRotation = agentTransform.rotation.eulerAngles;
        currentObservation[index++] = agentRotation.y / 360f;
        
        Vector3 agentVelocity = agentRigidbody.velocity;
        currentObservation[index++] = agentVelocity.x;
        currentObservation[index++] = agentVelocity.y;
        currentObservation[index++] = agentVelocity.z;

        // Ball observations
        if (ball != null)
        {
            Vector3 ballPosition = ball.transform.position;
            Vector3 relativeBallPosition = ballPosition - agentPosition;
            currentObservation[index++] = relativeBallPosition.x;
            currentObservation[index++] = relativeBallPosition.y;
            currentObservation[index++] = relativeBallPosition.z;
            
            Vector3 ballVelocity = ballRigidbody.velocity;
            currentObservation[index++] = ballVelocity.x;
            currentObservation[index++] = ballVelocity.y; 
            currentObservation[index++] = ballVelocity.z;

            currentObservation[index++] = relativeBallPosition.magnitude;
            
            Vector3 agentForward = agentTransform.forward;
            float angleToBall = Vector3.SignedAngle(agentForward, relativeBallPosition, Vector3.up) / 180f;
            currentObservation[index++] = angleToBall;
        }

        while (index < observationSize)
        {
            currentObservation[index++] = 0f;
        }

        UpdateMemory(currentObservation);
    }

    public void Reset() 
    {
        memoryQueue.Clear();
    }
}