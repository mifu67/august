using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using Ink.Runtime;
using System;

public class InteractiveDialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Intro messages file")]
    [SerializeField] private TextAsset messagesJSON;

    [Header("The Ink knot to load")]
    [SerializeField] private string knotName;
    [SerializeField] private string npcName;
    [SerializeField] private int flagNumber = -1;

    [SerializeField] private List<string> lastTurnsList = new List<string>();
    [SerializeField] private int numTopics;
    [SerializeField] private bool hasOutro = false;
    private List<ChatMessage> ConversationMessages = new List<ChatMessage>();

    private List<ChatMessage> RouterMessages = new List<ChatMessage>();

    private bool playerInRange;
    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        PopulateMessageList(messagesJSON, RouterMessages, "initial_router");
        PopulateMessageList(messagesJSON, ConversationMessages, "initial_character");
    }

    private void Update()
    {
        if (playerInRange && !InteractiveDialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if (InputManager.GetInstance().GetInteractPressed())
            {
                // enter dialogue mode
                StartCoroutine(InteractiveDialogueManager.GetInstance().EnterDialogueMode(RouterMessages, 
                ConversationMessages, lastTurnsList, inkJSON, npcName, numTopics, hasOutro, flagNumber, knotName));
            }
        }
        else 
        {
            visualCue.SetActive(false);
        }
    }

    private void PopulateMessageList(TextAsset messageJSON, List<ChatMessage> messageList, string knotName)
    {
        Story messages = new Story(messageJSON.text);
        messages.ChoosePathString(knotName);
        while (messages.canContinue)
        {
            string content = messages.Continue();
            string role = messages.currentTags[0];
            // Debug.Log(role);
            // Debug.Log(content);
            messageList.Add(
                new ChatMessage()
                {
                    Role = role,
                    Content = content
                }
            );
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
