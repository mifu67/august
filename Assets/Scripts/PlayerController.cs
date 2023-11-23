using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public Vector2 MovementSpeed = new Vector2(100.0f, 100.0f);
    public Animator animator;
    private new Rigidbody2D rigidbody2D;
    Vector2 inputVector;
    void Awake()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        
        rigidbody2D.angularDrag = 0.0f;
        rigidbody2D.gravityScale = 0.0f;
        animator.SetInteger("Direction", 0);
    }

    private void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Home")
        {
            if(MainManager.Instance.lastScene == "Larsen")
            {
                setPlayerPosition(-3.2f, -3f);
            }
        }
        else if (currentScene == "Larsen")
        {
            if(MainManager.Instance.lastScene == "Precinct")
            {
                setPlayerPosition(9.8f, -5f);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        if (DialogueManager.GetInstance().dialogueIsPlaying || InteractiveDialogueManager.GetInstance().dialogueIsPlaying || (NotebookScript.Instance != null && NotebookScript.Instance.getNotebookOpen()))
        // if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }
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

    private void setPlayerPosition(float x, float y)
    {
        transform.position = new Vector3(x, y, 0.0f);
    }
}
