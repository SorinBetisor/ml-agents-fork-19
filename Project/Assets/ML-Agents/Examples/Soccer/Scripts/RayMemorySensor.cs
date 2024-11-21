using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using System.Collections.Generic;
using System.Collections;
using System;
using Unity.Collections;
using Unity.Jobs;

public class RayMemorySensor : ISensor
{
    private float[] m_PastObservation;
    private Queue<float[]> MemoryQueue;
    public int MemorySize = 3;
    ObservationSpec m_ObservationSpec;
    float[] m_Observations;
    string m_Name;
    RayPerceptionSensor m_RayPerceptionSensor;

    /// <summary>
    /// Creates the RayMemorySensor.
    /// </summary>
    /// <param name="name">The name of the sensor.</param>
    /// <param name="rayInput">The inputs for the sensor.</param>
    public RayMemorySensor(string name, RayPerceptionSensor rayInput)
    {
        m_Name = name;
        m_RayPerceptionSensor = rayInput;
        MemoryQueue = new Queue<float[]>();
        m_PastObservation = new float[0];

        SetNumObservations(m_RayPerceptionSensor.RayPerceptionInput.OutputSize() * MemorySize);

        //initialize as queue with empty observations
        Reset();
    }

    void SetNumObservations(int numObservations)
    {
        m_ObservationSpec = ObservationSpec.Vector(numObservations);
        m_Observations = new float[numObservations];
    }

    /// <summary>
    /// Computes the ray perception observations and saves them to the provided
    /// <see cref="ObservationWriter"/>.
    /// </summary>
    /// <param name="writer">Where the ray perception observations are written to.</param>
    /// <returns></returns>
    public int Write(ObservationWriter writer)
    {
        Array.Clear(m_Observations, 0, m_Observations.Length);
        int index = 0;

        // For each observation, write the information to the observation writer
        foreach (var observation in MemoryQueue)
        {
            Array.Copy(observation, 0, m_Observations, m_RayPerceptionSensor.Observations.Length * index, observation.Length);
            index++;
        }

        writer.AddList(m_Observations);
        return m_Observations.Length;
    }

    /// <inheritdoc/>
    public void Update()
    {
        // Only enqueue if we have valid observations
        if (m_RayPerceptionSensor.Observations != null && m_RayPerceptionSensor.Observations.Length > 0)
        {
            // Make a copy of the current observation to store in memory
            m_PastObservation = new float[m_RayPerceptionSensor.Observations.Length];
            Array.Copy(m_RayPerceptionSensor.Observations, m_PastObservation, m_RayPerceptionSensor.Observations.Length);
            MemoryQueue.Enqueue(m_PastObservation);

            if (MemoryQueue.Count > MemorySize)
            {
                MemoryQueue.Dequeue();
            }
        }
    }

    /// <inheritdoc/>
    public void Reset()
    {
        MemoryQueue.Clear();
        m_PastObservation = new float[0];
        Array.Clear(m_PastObservation, 0, m_PastObservation.Length);

        for (int i = 0; i < MemorySize; i++)
        {
            var emptyObservation = new float[m_RayPerceptionSensor.Observations.Length];
            MemoryQueue.Enqueue(emptyObservation);
        }
    }

    /// <inheritdoc/>
    public ObservationSpec GetObservationSpec()
    {
        return m_ObservationSpec;
    }

    /// <inheritdoc/>
    public string GetName()
    {
        return m_Name;
    }

    /// <inheritdoc/>
    public virtual byte[] GetCompressedObservation()
    {
        return null;
    }

    /// <inheritdoc/>
    public CompressionSpec GetCompressionSpec()
    {
        return CompressionSpec.Default();
    }
}