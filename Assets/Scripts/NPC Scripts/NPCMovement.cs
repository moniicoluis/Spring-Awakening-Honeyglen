using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints defining the path
    public float moveSpeed = 3f; // Speed at which the NPC moves
    private int currentWaypointIndex = 0; // Index of the current waypoint
    private Animator animator; // Reference to the Animator component
    private bool isWalking = true;

    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        // Check if there are waypoints defined
        if (waypoints.Length > 0)
        {
            // Calculate movement direction for animation
            Vector2 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);



            // Move towards the current waypoint
            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, moveSpeed * Time.deltaTime);

            // Check if the NPC has reached the current waypoint
            if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
            {
                // Move to the next waypoint
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }



        }
    }

}
