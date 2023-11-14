using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using OpenAI;
public class InteractiveDialogueManager : MonoBehaviour
{
    const int ERIKA = 0;
    const int NPC = 1;
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

    private HashSet<string> explored_topics = new HashSet<string>();
    private int maxTopics = 0;
    private List<ChatMessage> routerMessages;
    private List<ChatMessage> conversationMessages;
    // will probably deserialize this field once I'm done debugging
    [SerializeField] private Queue<string> lastTurns = new Queue<string>();
    private string npcName = "";
    private string knotName = "";

    private int flagNumber = -1;

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
    private bool hasOutro = false;

    [SerializeField] private bool outroPlayed = false;

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
        endButton.onClick.AddListener(EndConversation);
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

    public IEnumerator EnterDialogueMode(List<ChatMessage> thisRouterMessages, List<ChatMessage> thisConversationMessages, List<string> thisLastTurns, 
                                         TextAsset inkJSON, string name, int numTopics, 
                                         bool thisHasOutro, int thisFlagNumber, string thisKnotName = "")
    {
        yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = true;
        currentStory = new Story(inkJSON.text);
        routerMessages = thisRouterMessages;
        conversationMessages = thisConversationMessages;
        lastTurns = new Queue<string>(thisLastTurns);
        knotName = thisKnotName;
        maxTopics = numTopics;
        hasOutro = thisHasOutro;
        flagNumber = thisFlagNumber;
        if (thisKnotName != "")
        {
            currentStory.ChoosePathString(thisKnotName);
        } else 
        {
            currentStory.ChoosePathString("intro");
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
    public void EndConversation()
    {
        if (hasOutro && !outroPlayed)
        {
            currentStory.ChoosePathString("outro");
            string currentSentence = currentStory.Continue();
            HandleTags(currentStory.currentTags);
            StartCoroutine(TypeSentence(currentSentence));
        } else 
        {
            ExitDialogueMode();
        }
        outroPlayed = true;
    }
    public void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        explored_topics = new HashSet<string>();
        playerTurn = false;
        dialoguePanel.SetActive(false);
        MainManager.Instance.SetFlagStatus(flagNumber, true);
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
                if (speakerNameText.text == "Erika")
                {
                    UpdateLastTurns(ERIKA, currentSentence);
                } else
                {
                    UpdateLastTurns(NPC, currentSentence);
                }
                StartCoroutine(TypeSentence(currentSentence));
            } else 
            {
                if (outroPlayed)
                {
                    ExitDialogueMode();
                }
                if (explored_topics.Count == maxTopics)
                {
                    EndConversation();
                } else 
                {
                    prewrittenMode = false;
                    playerTurn = true;
                    response.text = "";
                    speakerNameText.text = "Erika";
                    inputFieldObject.SetActive(true);
                }
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

    private string QueueToString(Queue<string> turns)
    {
        string context = "";
        foreach (string turn in turns)
        {
            context += turn;
            if (context[context.Length - 1] != '\n')
            {
                context += "\n";
            }
        }
        return context;
    }

    private void UpdateLastTurns(int speaker, string sentence)
    {
        string tag = speaker == ERIKA ? "E: " : "N: ";
        string turn = tag + sentence;
        if (lastTurns.Count >= 3)
        {
            lastTurns.Dequeue();
        }
        lastTurns.Enqueue(turn);
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
        string routerInputContent = QueueToString(lastTurns) + "\n" + userInput;
        // Debug.Log("CONTEXT: " + routerInputContent);
        UpdateLastTurns(ERIKA, userInput);
        ChatMessage routerInput = new ChatMessage()
        {
            Role = "user",
            Content = routerInputContent
        };
        routerMessages.Add(routerInput);
        var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-4", // use GPT-4 for router bc it's more powerful
            Messages = routerMessages,
            Temperature = 0.0f,
        });
        routerMessages.RemoveAt(routerMessages.Count - 1); // pop so as not to polute the context window
        string slot = completionResponse.Choices[0].Message.Content;
        Debug.Log("SLOT: " + slot);
        if (slot != "none")
        {
            // step 2: if the user input matches a prewritten response, add the correct response to messageList and 
            // play the prewritten conversation
            // at this step, add the correct input to the set
            if (slot.Substring(0, 5) != "bonus") 
            {
                explored_topics.Add(slot);
            }
            prewrittenMode = true;
            string currTopic = (knotName != "") ? knotName + "." + slot : slot;
            currentStory.ChoosePathString(currTopic);
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
                UpdateLastTurns(NPC, currentSentence);
            }
            else
            {
                currentSentence = "Sorry, could you say that again?";
            }
            speakerNameText.text = npcName;
        }
        // going to have to rework this section 
        playerTurn = false;
        ContinueStory();
    }

    private void NPCTurn()
    {
        // response.text = currentSentence;
        // Debug.Log(currentSentence);
        StartCoroutine(TypeSentence(currentSentence));
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

