using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 MovementSpeed = new Vector2(100.0f, 100.0f);
    public Animator animator;
    private new Rigidbody2D rigidbody2D;
    Vector2 inputVector;
    void Awake()
    {
        rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
        
        rigidbody2D.angularDrag = 0.0f;
        rigidbody2D.gravityScale = 0.0f;
        animator.SetInteger("Direction", 0);
    }
    // Update is called once per frame
    void Update()
    {
        // DIRECTION ENUM: down = 0, up = 1, left = 2, right = 3
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Horizontal", inputVector.x);
        animator.SetFloat("Vertical", inputVector.y);
        animator.SetFloat("Speed", inputVector.sqrMagnitude);

        if (inputVector.x < 0) {
            animator.SetInteger("Direction", 2);
        } else if (inputVector.x > 0) {
            animator.SetInteger("Direction", 3);
        } else if (inputVector.y < 0) {
            animator.SetInteger("Direction", 0);
        } else if (inputVector.y > 0) {
            animator.SetInteger("Direction", 1);
        }
    }

    void FixedUpdate()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + (inputVector * MovementSpeed * Time.fixedDeltaTime));
    }
}
