using UnityEngine;

public class ReverseTrigger : MonoBehaviour
{
    public Transform finishLine; // Reference to the Finish Line
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();
        TraceSpawnerManager traceSpawnerManager = GameObject.Find("TraceSpawner").GetComponent<TraceSpawnerManager>().Instance;

        if (traceSpawnerManager.EnoughTracesPlaced())
        {
            GameManager.EnterTransitionMode();
        }
        else
        {
            player.GameOver("You haven't placed enough traces! Press 'R' to restart.");
        }
    }
}
