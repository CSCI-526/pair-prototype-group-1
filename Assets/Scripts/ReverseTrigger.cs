using UnityEngine;

public class ReverseTrigger : MonoBehaviour
{
    // Reference to the starting point that the player should return to.
    public Transform startPoint;

    // When the player enters the trigger, activate reverse mode.
    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
{
    Debug.Log("Trigger entered by: " + other.gameObject.name);
    
    // Try getting the player_Controller from the collider or its parent.
    player_Controller playerCtrl = other.GetComponentInParent<player_Controller>();
    if (playerCtrl != null)
    {
        // Optionally, check the parent's tag:
        if (playerCtrl.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player detected in trigger.");
            Debug.Log("player_Controller component found. Activating reverse mode.");
            playerCtrl.ActivateReverseMode(startPoint.position.z);
        }
        else
        {
            Debug.Log("Parent object does not have the Player tag.");
        }
    }
    else
    {
        Debug.Log("player_Controller component not found on " + other.gameObject.name);
    }
     gameObject.SetActive(false);
}


}
