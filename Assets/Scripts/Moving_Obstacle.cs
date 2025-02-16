using UnityEngine;

public class Moving_Obstacle : MonoBehaviour
{
    public enum OscillationDirection { LeftToCenter, CenterToRight, LeftToRight } // Enum to define oscillation pairs
    public OscillationDirection direction; // The direction of oscillation

    public Transform leftLane;
    public Transform centerLane;
    public Transform rightLane;

    private Transform targetLane; // The lane moving towards
    private Transform startingLane; // Initial lane place position
    public float oscillationSpeed = 2.0f;

    public float rotationSpeed = 100.0f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        switch (direction)
        {
            case OscillationDirection.LeftToCenter:
                startingLane = leftLane;
                targetLane = centerLane;
                break;
                
            case OscillationDirection.CenterToRight:
                startingLane = centerLane;
                targetLane = rightLane;
                break;
                
            case OscillationDirection.LeftToRight:
                startingLane = leftLane;
                targetLane = rightLane;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Smooth Movement towards the target lane
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetLane.position.x, transform.position.y, transform.position.z), oscillationSpeed * Time.deltaTime);
        
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // obstacle reaches the target,and start moving in reverse direction and repeat
        if (Mathf.Abs(transform.position.x - targetLane.position.x) < 0.1f)
        {
            Transform temp = targetLane;
            targetLane = startingLane;
            startingLane = temp;
        }
    }
}
