using UnityEngine;

public class ReverseTrigger : MonoBehaviour
{
    public Transform finishLine; // sReference to the Finish Line

    // [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name);
        
        player_Controller playerCtrl = other.GetComponentInParent<player_Controller>();
        if (playerCtrl != null && playerCtrl.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player detected in trigger. Activating reverse mode.");
            
            // ✅ Activate reverse mode
            playerCtrl.ActivateReverseMode(finishLine.position.z);

            // ✅ Instead of destroying, disable the object
            gameObject.SetActive(false);
        }
    }
}
