using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [SerializeField] private GameObject dialogueTrigger;

    [SerializeField] private Button backButton;

    [SerializeField] private Button goButton;

    [SerializeField] private GameObject warning;

    private bool playerInRange;
    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        dialogueTrigger.SetActive(false);
    }

    private void Start()
    {
        backButton.onClick.AddListener(goBack);
        goButton.onClick.AddListener(enableDialogue);
    }

    private void Update()
    {
        if (playerInRange && !warning.activeSelf)
        {
            visualCue.SetActive(true);
            if (InputManager.GetInstance().GetInteractPressed())
            {
                warning.SetActive(true);
            }
        }
        else 
        {
            visualCue.SetActive(false);
        }
    }

    private void goBack()
    {   
        warning.SetActive(false);
    }

    private void enableDialogue()
    {
        warning.SetActive(false);
        dialogueTrigger.SetActive(true);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
