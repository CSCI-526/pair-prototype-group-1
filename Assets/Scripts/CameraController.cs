using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float transitionSpeedToReverseOffset = 3.5f; // 2 seconds reverse.
    void Update()
    {
        if (GameManager.gameState == GameManager.GameState.Transitioning)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                player.position + GameManager.cameraOffset,
                transitionSpeedToReverseOffset * Time.unscaledDeltaTime);
        }
    }
    void LateUpdate()
    {
        if (GameManager.gameState == GameManager.GameState.Transitioning)
        {
            if (Vector3.Distance(transform.position, player.position + GameManager.cameraOffset) < 0.01f)
                GameManager.EnterReverseMode();
        }
        else {
            transform.position = player.position + GameManager.cameraOffset;
        }
    }

    public void ForcedCameraUpdate() {
        transform.position = player.position + GameManager.cameraOffset;
    }
}