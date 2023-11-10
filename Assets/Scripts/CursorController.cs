using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    public Texture2D cursor;
    public Texture2D cursorClicked;

    private PlayerInput controls;
    private Camera mainCamera;

    private void Awake()
    {
        controls = new PlayerInput();
        ChangeCursor(cursor);
        Cursor.lockState = CursorLockMode.Confined;
        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    private void Start()
    {
        controls.Mouse.click.started += _ => StartedClick();
        controls.Mouse.click.performed += _ => EndedClick();
    }

    private void Update()
    {
        DetectObject();
    }
    
    private void DetectObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(controls.Mouse.position.ReadValue<Vector2>());
        // Debug.Log(controls.Mouse.position.ReadValue<Vector2>());

        RaycastHit2D hits2D = Physics2D.GetRayIntersection(ray);
        if (hits2D.collider != null)
        {
            ChangeCursor(cursorClicked);
        } else 
        {
            ChangeCursor(cursor);
        }
    }

    private void DetectObjectClicked()
    {
        Ray ray = mainCamera.ScreenPointToRay(controls.Mouse.position.ReadValue<Vector2>());
        // Debug.Log(controls.Mouse.position.ReadValue<Vector2>());

        RaycastHit2D hits2D = Physics2D.GetRayIntersection(ray);
        if (hits2D.collider != null)
        {
            Debug.Log("Hit 2D: " + hits2D.collider.tag);
            IClicked click = hits2D.collider.gameObject.GetComponent<IClicked>();
            if (click != null)
            {
                click.onClickAction();
            }
        }
    }
    private void StartedClick()
    {
        // ChangeCursor(cursorClicked);
    }

    private void EndedClick()
    {
        // ChangeCursor(cursor);

        DetectObjectClicked();
    }
    private void OnEnable() 
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    private void ChangeCursor(Texture2D cursorType)
    {
        Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);
    }
}
