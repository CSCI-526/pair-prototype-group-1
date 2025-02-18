using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed;
    public float laneChangeSpeed;
    public float jumpForce;
    public Rigidbody rb;
    private bool isGrounded = true; // Prevent double jumps.
    private int currentLane = Constants.Lanes.CENTER;

    // Variables for GameOver part
    public GameObject gameOverUI;
    public TMP_Text gameOverText; // Reference to Game Over message text
    public GameObject controlsUI;
    void Start()
    {
        RestartGame();
        Physics.gravity.Set(0, Constants.Forces.GRAVITY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Handle keyboard input.
        ProcessInput();

        if (GameManager.gameState == GameManager.GameState.Waiting)
        {
            return;
        }

        rb.linearVelocity =
            Vector3.forward * forwardSpeed * (GameManager.gameState == GameManager.GameState.ReverseMode ? -1 : 1) +
            new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, 0);
        rb.linearVelocity += Physics.gravity * Time.deltaTime;

        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(
                currentLane * Constants.Lanes.LANE_DISTANCE,
                transform.position.y,
                transform.position.z),
            laneChangeSpeed * Time.deltaTime);

        // Win condition.
        if (GameManager.gameState == GameManager.GameState.ReverseMode &&
            transform.position.z < Constants.WIN_POINT.z)
        {
            TraceSpawnerManager traceSpawnerManager = GameObject.Find("TraceSpawner").GetComponent<TraceSpawnerManager>().Instance;
            if (traceSpawnerManager.EnoughTracesCollected())
            {
                GameOver("You Win! Press 'R' to play again.");
                gameOverText.color = Color.green;
            }
            else
            {
                GameOver("You have not collected enough traces :( Press 'R' to restart.");
            }
        }

        // Check if player has passed the reverse collectible.
        if (GameManager.gameState == GameManager.GameState.NormalMode &&
            transform.position.z > GameManager.reverseCollectible.transform.position.z + 1.0f)
        {
            GameOver("You missed the reverser! Press 'R' to restart.");
        }
    }

    void ProcessInput()
    {
        if (GameManager.gameState == GameManager.GameState.Waiting ||
            GameManager.gameState == GameManager.GameState.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space) && GameManager.gameState == GameManager.GameState.Waiting)
            {
                controlsUI.SetActive(false);
                GameManager.EnterNormalMode();
            }

            if (Input.GetKeyDown(KeyCode.R) && GameManager.gameState == GameManager.GameState.GameOver)
            {
                RestartGame();
            }

            return;
        }

        // Change lanes.
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentLane -= 1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentLane += 1;
        }
        currentLane = Math.Clamp(currentLane, -1, 1);

        // Jump.
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    // Collision detection for ground and obstacles.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.normal.y != 0)
                {
                    Debug.Log("Landed on top of the obstacle, continue running!");
                    isGrounded = true;
                }
                else
                {
                    GameOver("You crashed! Press 'R' to restart.");
                }
            }
        }
        else if (collision.gameObject.CompareTag("Obstacle_Type2"))
        {
            if (transform.position.y > 1.4f)
            {
                Debug.Log("Landed on top of the obstacle, continue running!");
                isGrounded = true;
            }
            else
            {
                GameOver("You crashed! Press 'R' to restart.");
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle"))
        {
            isGrounded = false;
        }
    }

    // Restart game logic.
    void RestartGame()
    {
        gameOverUI.SetActive(false);
        gameOverText.color = Color.white;
        controlsUI.SetActive(true);
        ResetPlayerState();
        GameManager.EnterWaitingMode();
    }

    // Game Over logic.
    public void GameOver(string gameOverMessage)
    {
        GameManager.EnterGameOverMode();
        gameOverUI.SetActive(true);
        gameOverText.text = gameOverMessage;
    }

    void ResetPlayerState()
    {
        transform.position = Constants.SPAWN_POINT;
        currentLane = Constants.Lanes.CENTER;
        rb.linearVelocity = Vector3.zero;
        Camera.main.GetComponent<CameraController>().ForcedCameraUpdate();
    }
}
