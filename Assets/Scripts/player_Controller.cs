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

    // Variables for GameOver part
    private Vector3 startPosition; 
    public GameObject gameOverUI;
    public TMP_Text gameOverText; // Reference to Game Over message text
    public GameObject finalCollectible; // ✅ Assign this in Unity Inspector

    // Reverse mode variables
    private bool isReverseMode = false;
    private float reverseEndZ; // The z-coordinate to stop reverse mode (starting point)
    public Transform startPoint;
    // Start is called before the first frame update
    public GameObject controlsUI;
    void Start()
    {
        isPlay = false;
        currentPosition = 0; // currentPosition=0(center), =1(left), =2(right)
        startPosition = transform.position; // Store starting position for reverse target.

         if (controlsUI != null)
        {
            controlsUI.SetActive(true);
        }
    }

    // Update is called once per frame
    // [System.Obsolete]
    void Update()
{
    rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

    if(Input.GetMouseButtonDown(0)){
        isPlay = true;
         if (controlsUI != null)
        {
            controlsUI.SetActive(false);
        }
    }
    if(Input.GetKeyDown(KeyCode.R) && Time.timeScale == 0 ){
        RestartGame();
    }
    if(isPlay){
        // Update the player's forward movement. In reverse mode forward_speed is negative.
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, forward_speed);

        // Lateral movement: move toward the chosen lane.
        if(currentPosition == 0){
            transform.position = Vector3.Lerp(transform.position, new Vector3(Center_position.position.x, transform.position.y, transform.position.z), side_Speed * Time.deltaTime);
        }  
        else if(currentPosition == 1){
            transform.position = Vector3.Lerp(transform.position, new Vector3(Left_position.position.x, transform.position.y, transform.position.z), side_Speed * Time.deltaTime);
        }
        else if(currentPosition == 2){
            transform.position = Vector3.Lerp(transform.position, new Vector3(Right_position.position.x, transform.position.y, transform.position.z), side_Speed * Time.deltaTime);
        }
        
        // Change lanes using left/right arrow keys.
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            if(currentPosition == 0){
                currentPosition = 1;
            }
            else if(currentPosition == 2){
                currentPosition = 0;
            }
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            if(currentPosition == 0){
                currentPosition = 2;
            }
            else if(currentPosition == 1){
                currentPosition = 0;
            }
        }

        // Jump logic
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, JumpForce, rb.linearVelocity.z);
            isGrounded = false;
        }

        
    }
}
// Winning condition for the game
void OnTriggerEnter(Collider other)
{
    // ✅ Check if the collided object's Transform matches the startPoint Transform
    if (other.transform == startPoint)
    {
        EndGame(); // ✅ Call game over logic when reaching the finish line
    }
}

void EndGame()
{
    Debug.Log("Game Over: Player reached the finish line!");
    
    isPlay = false; // Stop player movement
    rb.linearVelocity = Vector3.zero; // Stop all movement

    // ✅ Show the Game Over UI
    gameOverUI.SetActive(true);
    gameOverText.text = "You Win! Press 'R' to Restart.";
    
    Time.timeScale = 0; // Pause the game
}



    // Collision detection for ground and obstacles.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Obstacle_Type2"))
        {
            float playerBottom = transform.position.y - (transform.localScale.y / 2);
            float obstacleTop = collision.transform.position.y + (collision.transform.localScale.y / 2);
            if (playerBottom >= obstacleTop - 0.1f)
            {
                Debug.Log("Landed on the obstacle, continue running!");
                isGrounded = true;
            }
            else
            {
                GameOver();
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
        Time.timeScale = 1;
        gameOverUI.SetActive(false);
        transform.position = startPosition;
        rb.linearVelocity = Vector3.zero;
        isPlay = false;
        forward_speed = Mathf.Abs(forward_speed);
        isReverseMode=false;
        if (finalCollectible != null && !finalCollectible.activeSelf)
    {
        finalCollectible.SetActive(true);
    }

    }

    // Game Over logic.
    void GameOver()
    {
        Debug.Log("Collision from front! Game Over.");
        isPlay = false;
        rb.linearVelocity = Vector3.zero;
        transform.position = startPosition;
        gameOverUI.SetActive(true);
        gameOverText.text = "You crashed! Press 'R' to restart.";
        Time.timeScale = 0;
    }

   
    public void ActivateReverseMode(float endZ)
{
    isReverseMode = true;
    reverseEndZ = endZ;
    forward_speed = -Mathf.Abs(forward_speed);
    Debug.Log("Reverse mode activated. New forward_speed: " + forward_speed);

  
}

}
