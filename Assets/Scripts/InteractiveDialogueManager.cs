using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveDialogueManager : MonoBehaviour
{
    [SerializeField] private Button endButton;
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI response;
    [SerializeField] private GameObject inputFieldObject;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI speakerNameText;

    [SerializeField] private Animator portrait1Animator;
    [SerializeField] private Animator portrait2Animator;

    [SerializeField] private Image sprite1;
    [SerializeField] private Image sprite2;

    private string npcName = "";

    [SerializeField]
    private float textSpeed;

    // will change this to a Story later, but this is okay for now
    private string currentSentence;
    public bool dialogueIsPlaying { get; private set; }
    private static InteractiveDialogueManager instance;

    private bool playerTurn = false;

    private void Awake()
    {

        if (instance != null)
        {
            Debug.Log("Found more than one Interactive Dialogue Manager in the scene.");
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
        inputField.onSubmit.AddListener(GetResponse);
        endButton.onClick.AddListener(ExitDialogueMode);
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }
        if (InputManager.GetInstance().GetSubmitPressed() && !playerTurn)
        {
            playerTurn = true;
            ContinueStory();
        }
    }

    public void EnterDialogueMode(string introLine, string name) // will eventually need to use ink, but this is ok for mvp
    {
        inputField.text = "";
        npcName = name;
        speakerNameText.text = npcName;
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        currentSentence = introLine;

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        playerTurn = false;
        dialoguePanel.SetActive(false);
        response.text = "";
    }
    private void ContinueStory()
    {
        if (playerTurn) {
            response.text = "";
            speakerNameText.text = "Erika";
            inputFieldObject.SetActive(true);
            inputField.text = "<i>What should I say?</i>";
        } else 
        {
            inputFieldObject.SetActive(false);
            NPCTurn();
        }
    }
    private void GetResponse(string msg)
    {
        if (inputField.text.Length < 1)
        {
            return;
        }
        Debug.Log("Player pressed submit");
        // api call would be here
        currentSentence = "This is a placeholder sentence.";
        speakerNameText.text = npcName;
        playerTurn = false;
    }

    private void NPCTurn()
    {
        response.text = currentSentence;
    }
}
