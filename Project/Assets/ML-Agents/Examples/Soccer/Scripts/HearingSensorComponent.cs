using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;

/// <summary>
/// A simple example of a SensorComponent.
/// This should be added to the same GameObject as the BasicController
/// </summary>
public class HearingSensorComponent : SensorComponent
{
    public PlayerHearingSensor m_PlayerHearingSensor;


    /// <summary>
    /// Creates a BasicSensor.
    /// </summary>
    /// <returns></returns>
    public override ISensor[] CreateSensors()
    {
        m_PlayerHearingSensor = new PlayerHearingSensor(gameObject);
        return new ISensor[] {m_PlayerHearingSensor };
    }

    public PlayerHearingSensor GetHearingSensor() {
        return m_PlayerHearingSensor;
    }
}
