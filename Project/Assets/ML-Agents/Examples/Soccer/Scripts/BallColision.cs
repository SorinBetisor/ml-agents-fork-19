using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public GameObject area;
    [HideInInspector]
    public SoccerEnvController envController;
    

    void Start()
    {
    
        envController = area.GetComponent<SoccerEnvController>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("purpleAgent") || col.gameObject.CompareTag("blueAgent") || col.gameObject.CompareTag("wall"))
        {
            envController.NotifyPlayersHearing(transform.position);
        }
    }
}
