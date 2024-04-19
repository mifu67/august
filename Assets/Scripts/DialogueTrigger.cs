using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("The Ink knot to load")]
    [SerializeField] private string knotName;

    [Header("Scene to Load")]
    [SerializeField] private string sceneToLoad;

    [Header("-1 if there is no associated piece of evidence")]
    [SerializeField] private int evidenceIndex = -1;
    private bool playerInRange;
    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if (!MainManager.Instance.EventFlagTriggered(1))
        {
            // Player hasn't talked to Adrian yet
            return;
        }
        
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if (InputManager.GetInstance().GetInteractPressed())
            {
                StartCoroutine(DialogueManager.GetInstance().EnterDialogueMode(inkJSON, knotName, sceneToLoad, evidenceIndex));
            }
        }
        else 
        {
            visualCue.SetActive(false);
        }
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
