using Unity.MLAgents.Sensors;
using UnityEngine;

public class HearingSensor : ISensor
{
    private PlayerHearing playerHearing;
    private string sensorName;

    public HearingSensor(PlayerHearing hearing, string name = "HearingSensor")
    {
        playerHearing = hearing;
        sensorName = name;
    }

    public string GetName()
    {
        return sensorName;
    }

    public ObservationSpec GetObservationSpec()
    {
        // Define an observation spec for a vector of 3 (x, y, z positions of the sound)
        return ObservationSpec.Vector(3);
    }

    public byte[] GetCompressedObservation()
    {
        return null; // No compression used for this sensor
    }

    public CompressionSpec GetCompressionSpec()
    {
        return CompressionSpec.Default();
    }

    public void Reset()
    {
        // No specific reset needed; handled in PlayerHearing
    }

    public void Update()
    {
        // No internal updates needed per frame
    }

    public int Write(ObservationWriter writer)
    {
        if (playerHearing.GetHeardStatus(out Vector3 soundPosition))
        {
            // Write each component of soundPosition individually if a sound was heard
            writer.Add(soundPosition);
        }
        else
        {
            // Write zeros if no sound was heard
            writer.Add(new Vector3(0f, 0f, 0f));
        }
        return 1; // Number of elements written (x, y, z)
    }

}
