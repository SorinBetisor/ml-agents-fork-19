using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;
using UnityEngine.Serialization;

/// <summary>
/// A class to support sensor components for raycast memory based sensors.
/// </summary>
public class RayMemoryComponent : SensorComponent
{
    [HideInInspector, SerializeField, FormerlySerializedAs("sensorName")]
    string m_SensorName = "RayMemorySensor";

    /// <summary>
    /// The name of the Sensor that this component wraps.
    /// Note that changing this at runtime does not affect how the Agent sorts the sensors.
    /// </summary>
    public string SensorName
    {
        get { return m_SensorName; }
        set { m_SensorName = value; }
    }

    [NonSerialized]
    RayMemorySensor m_RayMemorySensor;

    /// <summary>
    /// Get the RayMemorySensor that was created.
    /// </summary>
    public RayMemorySensor MemorySensor
    {
        get => m_RayMemorySensor;
    }

    /// <summary>
    /// Create the ISensors. This is called by the Agent when it is initialized.
    /// </summary>
    public override ISensor[] CreateSensors()
    {
        m_RayMemorySensor = new RayMemorySensor(m_SensorName, gameObject.GetComponent<RayPerceptionSensorComponent3D>().RaySensor);

        return new ISensor[] { m_RayMemorySensor };
    }
}
