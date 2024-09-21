using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    public bool isMoving;

    private Vector2 input;

    private Animator animator;

    public LayerMask solidObjectsLayer;

    public LayerMask interactableLayer;

    public VectorValue startingPosition;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        transform.position = startingPosition.initialValue;
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        if (!isMoving)
        {
            //Get input and assign to variable
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //Debug.Log("This is input.x " + input.x);
            //Debug.Log("This is input.y " + input.y);

            if (input.x != 0)
            {
                input.y = 0;
            }


            //If there is any input
            if (input != Vector2.zero)
            {
                //Set animation in accordance with the input
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                //Change the position of the character based on input
                var targetPosition = transform.position;
                targetPosition.x += input.x;
                targetPosition.y += input.y;

                if (IsWalkable(targetPosition))
                {
                    StartCoroutine(Move(targetPosition));
                }

            }

            animator.SetBool("isMoving", isMoving);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        //Debug.DrawLine(transform.position, interactPos, Color.red, 1f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    public IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 6f;
        }
        else
        {
            moveSpeed = 4f;
        }

        while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;

        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPosition)
    {
        if (Physics2D.OverlapCircle(targetPosition, 0.2f, solidObjectsLayer | interactableLayer) != null)
        {
            return false;
        }

        return true;
    }
}
