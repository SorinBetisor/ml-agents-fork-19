using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHearing : MonoBehaviour
{
    public void ReceiveSignal(Vector3 ballPosition)
    {
        Debug.Log(gameObject.name + " heard the ball collision at " + ballPosition);
    }
}
