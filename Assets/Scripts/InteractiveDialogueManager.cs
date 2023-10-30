using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using OpenAI;

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
        if (InputManager.GetInstance().GetInteractPressed() && playerTurn)
        {
            GetResponse();
        }
    }

    public IEnumerator EnterDialogueMode(string introLine, string name) // will eventually need to use ink, but this is ok for mvp
    {
        yield return new WaitForSeconds(0.2f);
        inputField.text = "";
        npcName = name;
        speakerNameText.text = npcName;
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        currentSentence = introLine;

        inputField.text = "";
        inputFieldObject.SetActive(false);
        ContinueStory();
    }

    public void ExitDialogueMode()
    {
        Debug.Log("Exiting Dialogue Window");
        dialogueIsPlaying = false;
        playerTurn = false;
        dialoguePanel.SetActive(false);
        response.text = "";
    }
    private void ContinueStory()
    {
        Debug.Log(playerTurn);

        if (playerTurn) {
            response.text = "";
            speakerNameText.text = "Erika";
            inputFieldObject.SetActive(true);
        } else 
        {
            NPCTurn();
        }
    }
    public async void GetResponse()
    {
        if (inputField.text.Length < 1)
        {
            return;
        }
        speakerNameText.text = npcName;
        inputField.text = "";
        inputFieldObject.SetActive(false);
        var openai = new OpenAIApi();
        var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-3.5-turbo",
            Messages = new List<ChatMessage>
            {
                new ChatMessage()
                {
                    Role = "user",
                    Content = "Please say a random quote from a Shakespeare play."
                }
            },
            Temperature = 0.5f,
        });

        if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
        {
            currentSentence = completionResponse.Choices[0].Message.Content;
        }
        else
        {
            currentSentence = "This didn't work.";
        }
        // currentSentence = "This is a placeholder sentence.";
        speakerNameText.text = npcName;
        playerTurn = false;
        ContinueStory();
    }

    private void NPCTurn()
    {
        response.text = currentSentence;
    }
}

