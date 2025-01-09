using UnityEngine;
using Unity.MLAgents.Sensors;
using UnityEngine.Serialization;

public class RayMemoryComponent : SensorComponent
{
    [SerializeField]
    [Range(1, 50)]
    [Tooltip("Number of raycast observations that will be stacked before being fed to the neural network.")]
    int m_ObservationStacks = 2;

    private ISensor m_RayPerceptionSensor;

    public override ISensor[] CreateSensors()
    {
        var rayPerceptionComponent = gameObject.GetComponent<RayPerceptionSensorComponent3D>();
        if (rayPerceptionComponent == null)
        {
            Debug.LogError($"RayMemoryComponent on {gameObject.name} requires a RayPerceptionSensorComponent3D but none was found.");
            return System.Array.Empty<ISensor>();
        }

        m_RayPerceptionSensor = rayPerceptionComponent.RaySensor;

        // If stacking is enabled, wrap the sensor in a StackingSensor
        if (m_ObservationStacks > 1)
        {
            // string uniqueName = $"{m_SensorName}_{gameObject.name}";
            return new ISensor[] { new StackingSensor(m_RayPerceptionSensor, m_ObservationStacks) };
        }

        return new ISensor[] { m_RayPerceptionSensor };
    }
}