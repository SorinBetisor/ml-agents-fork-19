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
    public int ModelNum;

    /// <summary>
    /// Creates a BasicSensor.
    /// </summary>
    /// <returns></returns>
    
    public override ISensor[] CreateSensors()
    {
        switch(ModelNum){
            case 1:
                m_PlayerHearingSensor = new PlayerHearingSensor(gameObject, PlayerHearingSensor.Model.Coordinates);
                return new ISensor[] {m_PlayerHearingSensor };

            case 2:
                m_PlayerHearingSensor = new PlayerHearingSensor(gameObject, PlayerHearingSensor.Model.RelativeNormalization);
                return new ISensor[] {m_PlayerHearingSensor };

            case 3:
                m_PlayerHearingSensor = new PlayerHearingSensor(gameObject, PlayerHearingSensor.Model.RelativeRotationNormalization);
                return new ISensor[] {m_PlayerHearingSensor };

            case 4:
                m_PlayerHearingSensor = new PlayerHearingSensor(gameObject, PlayerHearingSensor.Model.RelativeRotation);
                return new ISensor[] {m_PlayerHearingSensor };
            default:
                m_PlayerHearingSensor = new PlayerHearingSensor(gameObject, PlayerHearingSensor.Model.Coordinates);
                return new ISensor[] {m_PlayerHearingSensor };           
        }
    }

    public PlayerHearingSensor GetHearingSensor() {
        return m_PlayerHearingSensor;
    }
}
