using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
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
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerNameText;

    [SerializeField] private Animator portraitAnimator;
    [SerializeField] private Image sprite;

    private List<ChatMessage> routerMessages;
    private List<ChatMessage> conversationMessages;
    [SerializeField] private string lastUtterance = "";
    private string npcName = "";

    [SerializeField]
    private float textSpeed;

    private Story currentStory;
    [SerializeField]
    private string currentSentence;
    public bool dialogueIsPlaying { get; private set; }
    private static InteractiveDialogueManager instance;

    private bool isAddingRichTextTag = false;
    private bool playerTurn = false;
    private bool prewrittenMode = true;

    private OpenAIApi openai = new OpenAIApi();
    private const string PORTRAIT_TAG = "portrait";
    private const string SPEAKING_TAG = "speaking";
    private const string COLOR_TAG = "color";

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
            if (!prewrittenMode) 
            {
                playerTurn = true;
            }
            ContinueStory();
        }
        if (InputManager.GetInstance().GetInteractPressed() && playerTurn)
        {
            GetResponse();
        }
    }

    public IEnumerator EnterDialogueMode(List<ChatMessage> thisRouterMessages, List<ChatMessage> thisConversationMessages, string thisLastUtterance, TextAsset inkJSON, string name, string knotName = "")
    {
        yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = true;
        currentStory = new Story(inkJSON.text);
        routerMessages = thisRouterMessages;
        conversationMessages = thisConversationMessages;
        lastUtterance = thisLastUtterance;
        if (knotName != "")
        {
            currentStory.ChoosePathString(knotName);
        }
        inputField.text = "";
        npcName = name;
        speakerNameText.text = npcName;
        dialoguePanel.SetActive(true);
        // begin by playing the intro ink
        inputField.text = "";
        inputFieldObject.SetActive(false);
        ContinueStory();
    }

    IEnumerator TypeSentence(string sentence) 
    {
        dialogueText.text = sentence;
        dialogueText.maxVisibleCharacters = 0;
        foreach (char letter in sentence.ToCharArray()) {
            if (letter == '<' || isAddingRichTextTag) {
                isAddingRichTextTag = true;
                if (letter == '>') {
                    isAddingRichTextTag = false;
                }
            }
            else {
                dialogueText.maxVisibleCharacters ++;
                yield return new WaitForSeconds(textSpeed);
            }
        }
    }
    public void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        playerTurn = false;
        dialoguePanel.SetActive(false);
        prewrittenMode = true;
        response.text = "";
    }
    private void ContinueStory()
    {
        if (prewrittenMode)
        {
            if (currentStory.canContinue)
            {
                string currentSentence = currentStory.Continue();
                HandleTags(currentStory.currentTags);
                StartCoroutine(TypeSentence(currentSentence));
            } else 
            {
                prewrittenMode = false;
                playerTurn = true;
                response.text = "";
                speakerNameText.text = "Erika";
                inputFieldObject.SetActive(true);
            }  
        } else
        {
            if (playerTurn) {
                response.text = "";
                speakerNameText.text = "Erika";
                inputFieldObject.SetActive(true);
            } else 
            {
                NPCTurn();
            }
        }
    }

    public async void GetResponse()
    {
        if (inputField.text.Length < 1)
        {
            return;
        }
        speakerNameText.text = npcName;
        string userInput = inputField.text;
        inputField.text = "";
        inputFieldObject.SetActive(false);
        response.text = "...";
        // step 1: pass the user input to the router model—remember to pop from list
        string routerInputContent = lastUtterance + "\n\n" + userInput;
        ChatMessage routerInput = new ChatMessage()
        {
            Role = "user",
            Content = routerInputContent
        };
        routerMessages.Add(routerInput);
        Debug.Log(routerInputContent);
        var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-4", // use GPT-4 for router bc it's more powerful
            Messages = routerMessages,
            Temperature = 0.0f,
        });
        string slot = completionResponse.Choices[0].Message.Content;
        Debug.Log("SLOT" + slot);
        if (slot != "none")
        {
            // step 2: if the user input matches a prewritten response, add the correct response to messageList and 
            // play the prewritten conversation
            // at this step, add the correct input to the set; quit early if all required paths explored
            currentSentence = "We'll be doing some routing here.";
        } 
        else 
        {
            // if the user input doesn't match a prewritten response, pass the user utterance to the convo model
            ChatMessage convoInput = new ChatMessage()
            {
                Role = "user",
                Content = userInput
            };
            conversationMessages.Add(convoInput);
            completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo", 
                Messages = conversationMessages,
                Temperature = 0.0f,
            });
            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                currentSentence = completionResponse.Choices[0].Message.Content;
            }
            else
            {
                currentSentence = "Sorry, could you say that again?";
            }
        }
        // going to have to rework this section 
        speakerNameText.text = npcName;
        playerTurn = false;
        ContinueStory();
    }

    private void NPCTurn()
    {
        response.text = currentSentence;
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            // parse the tag
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            // handle the tag
            switch (tagKey)
            {
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                case SPEAKING_TAG:
                    if (tagValue == "erika")
                    {
                        speakerNameText.text = "Erika";
                    } else if (tagValue == "npc")
                    {
                        speakerNameText.text = npcName;
                    } else 
                    {
                        Debug.LogWarning("Invalid speaker: " + tagValue);
                    }
                    break;
                case COLOR_TAG:
                    setTextColor(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but has no handler: " + tag);
                    break;
            }
        }
    }

    private void setTextColor(string color)
    {
        switch(color)
        {
            case "navy":
                dialogueText.color = new Color(0.110f, 0.217f, 0.537f);
                break;
            default:
                dialogueText.color = Color.black;
                break;
        }
    }
}

