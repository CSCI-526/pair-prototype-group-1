using UnityEngine;
using System.Collections;
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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isPlay = false;
        // currentPosition=0(center), =1(left), =2(right)
        currentPosition = 0;

    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        if(Input.GetMouseButtonDown(0)){
            isPlay = true;
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

            // Jumping logic    02
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, JumpForce, rb.linearVelocity.z); // Apply jump force
                isGrounded = false; // Mark as not grounded
            }


            // if(Input.GetKeyDown(KeyCode.Space)){
            //     rb.AddForce(Vector3.up * JumpForce , ForceMode.Impulse);
            //     // StartCoroutine(Forward_Flip_jump());
            // } 
            //01 this works: 

        }


    }

    // Detect collision with ground
    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is touching the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Reset isGrounded when touching the ground
        }
    }

    // IEnumerator Forward_Flip_jump()
    // {
    //     yield return new WaitForSeconds(0.01f);
    // }

}

