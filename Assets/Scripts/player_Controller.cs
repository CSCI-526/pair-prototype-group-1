using UnityEngine;
using System.Collections;
using TMPro;


public class player_Controller : MonoBehaviour
{
    public float forward_speed;
    public Transform Center_position;
    public Transform Left_position;
    public Transform Right_position;

    public float side_Speed;
    private int currentPosition;
    private bool isPlay;

    public float JumpForce;
    public Rigidbody rb;
    public float gravity = -9.8f;
    private bool isGrounded = true; // Flag to track if player is on the ground

    // variables for GameOver part
    private Vector3 startPosition; 
    public GameObject gameOverUI;
    public TMP_Text gameOverText; // Reference to Game Over message text

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isPlay = false;
        currentPosition = 0; // currentPosition=0(center), =1(left), =2(right)
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        if(Input.GetMouseButtonDown(0)){
            isPlay = true;
        }
        if(Input.GetKeyDown(KeyCode.R) && Time.timeScale == 0 ){
            RestartGame();
        }
        if(isPlay){
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + forward_speed * Time.deltaTime);   
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, forward_speed);
            if(currentPosition == 0){
                transform.position = Vector3.Lerp(transform.position, new Vector3(Center_position.position.x, transform.position.y, transform.position.z), side_Speed * Time.deltaTime);
            }  
            else if(currentPosition == 1){
                transform.position = Vector3.Lerp(transform.position, new Vector3(Left_position.position.x, transform.position.y, transform.position.z), side_Speed * Time.deltaTime);
            }
            else if(currentPosition == 2){
                transform.position = Vector3.Lerp(transform.position, new Vector3(Right_position.position.x, transform.position.y, transform.position.z), side_Speed * Time.deltaTime);
            }
            // leftArrow Press
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                if(currentPosition == 0){
                    currentPosition = 1;
                }
                else if(currentPosition == 2){
                    currentPosition = 0;
                }
            }
            // rightArrow Press
            if(Input.GetKeyDown(KeyCode.RightArrow)){
                if(currentPosition == 0){
                    currentPosition = 2;
                }
                else if(currentPosition == 1){
                    currentPosition =0;
                }
            }
            // Jump logic
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, JumpForce, rb.linearVelocity.z); // Apply jump force
                isGrounded = false; // Mark as not grounded
            }
        }
    }

    // Detecting the collision with ground to control jumping limitation
    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is touching the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Resetting the value of  isGrounded on groundTouch
        }
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Obstacle_Type2"))
        {
            float playerBottom = transform.position.y - (transform.localScale.y / 2);
            float obstacleTop = collision.transform.position.y + (collision.transform.localScale.y / 2);

            // If the player's bottom is above the obstacle's top, allow them to land on it
            if (playerBottom >= obstacleTop - 0.1f)
            {
                Debug.Log("Landed on the obstacle, continue running!");
                isGrounded = true; // Treat obstacle as ground
            }
            else
            {
                GameOver();
            }
        }

    }
    // Detecting the collisionn with obstacle and stopping the game
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle"))
        {
            isGrounded = false;
        }
    }

    // restart the game
    void RestartGame()
    {
        Time.timeScale = 1; // Resume game
        gameOverUI.SetActive(false); // Hide Game Over UI
        transform.position = startPosition; // Reset player position
        rb.linearVelocity = Vector3.zero;
        isPlay = false; // Set play state to false (waiting for input)
    }

    // Game Over function
    void GameOver()
    {
        Debug.Log("Collision from front! Game Over.");
        isPlay = false; // Stop player movement
        rb.linearVelocity = Vector3.zero; // Reset the value of velocity to starting velocity
        transform.position = startPosition; // Reset position of the player
        gameOverUI.SetActive(true); // Show the UI that the Game is Over 
        gameOverText.text = "You crashed! Press 'R' to restart."; // Display the message to guide player to restart
        Time.timeScale = 0; // Pause the game

    }

}

