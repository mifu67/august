using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveDialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerNameText;

    [SerializeField] private Animator portrait1Animator;
    [SerializeField] private Animator portrait2Animator;

    [SerializeField] private Image sprite1;
    [SerializeField] private Image sprite2;

    [SerializeField]
    private float textSpeed;
    public bool dialogueIsPlaying { get; private set; }
    private static InteractiveDialogueManager instance;

    private void Awake()
    {

        if (instance != null)
        {
            Debug.Log("Found more than one Dialogue Manager in the scene.");
            return;
        }
        instance = this;
        }
    public static InteractiveDialogueManager GetInstance()
    {
        return instance;
    }
    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }
}
