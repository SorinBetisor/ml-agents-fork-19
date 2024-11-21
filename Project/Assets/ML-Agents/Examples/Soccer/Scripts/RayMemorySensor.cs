using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using System.Collections.Generic;
using System;
using Unity.Collections;
using Unity.Jobs;

public class RayMemorySensor : ISensor
{
    private float[] m_CurrentObservation;
    private Queue<float[]> MemoryQueue;
    public int MemorySize = 2;
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
        m_CurrentObservation = new float[0];

        SetNumObservations(m_RayPerceptionSensor.RayPerceptionInput.OutputSize() * MemorySize);
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
        // int index = 0;

        // For each observation, write the information to the observation writer
        // foreach (var observation in MemoryQueue)
        // {
        //     m_Observations.CopyTo(m_Observations, m_RayPerceptionSensor.Observations.Length * index++);
        // }

        writer.AddList(m_Observations);
        return m_Observations.Length;
    }

    /// <inheritdoc/>
    public void Update()
    {
        if (m_CurrentObservation.Length > 0)
        {
            MemoryQueue.Enqueue(m_CurrentObservation);
        }

        if (MemoryQueue.Count > MemorySize)
        {
            MemoryQueue.Dequeue();
        }

        m_CurrentObservation = m_RayPerceptionSensor.Observations;
    }

    /// <inheritdoc/>
    public void Reset() { }

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