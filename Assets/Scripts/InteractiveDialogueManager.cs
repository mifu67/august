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
    [SerializeField] private bool convinced = false;
    [SerializeField] private bool gettingAnswer = true;
    [SerializeField] private bool deductionMode = false;
    [SerializeField] private bool tutorialMode = false;
    private bool shouldDestroy = false;
    private GameObject npc;
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
    private Story currentMessages;
    [SerializeField]
    private string currentSentence;
    public bool dialogueIsPlaying { get; private set; }
    private static InteractiveDialogueManager instance;

    private bool isAddingRichTextTag = false;
    private bool canContinueToNextLine = false;
    private bool submitButtonPressedThisFrame = false;
    [SerializeField] private bool playerTurn = false;
    [SerializeField] private bool prewrittenMode = true;
    [SerializeField] private bool hasOutro = false;

    [SerializeField] private bool outroPlayed = false;

    [SerializeField]
    private AudioClip dialogueTypingSoundClip;

    [SerializeField]
    private int frequency = 2;

    private AudioSource audioSource;

    private OpenAIApi openai = new OpenAIApi();
    private const string PORTRAIT_TAG = "portrait";
    private const string SPEAKING_TAG = "speaking";
    private const string COLOR_TAG = "color";

    private const string TOOLTIP_TAG = "tooltip";
    private void Awake()
    {

        if (instance != null)
        {
            Debug.Log("Found more than one Interactive Dialogue Manager in the scene.");
            return;
        }
        instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();
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
        if (NotebookScript.Instance == null || NotebookScript.Instance.getNotebookOpen() == false)
        {
            if (InputManager.GetInstance().GetSubmitPressed()) {
                submitButtonPressedThisFrame = true;
            }
            if (submitButtonPressedThisFrame && canContinueToNextLine && !playerTurn)
            {
                if (!prewrittenMode) 
                {
                    playerTurn = true;
                }
                submitButtonPressedThisFrame = false;
                ContinueStory();
            }
            if (InputManager.GetInstance().GetInteractPressed() && playerTurn)
            {
                submitButtonPressedThisFrame = false; // submit and interact have overlap
                if (deductionMode)
                {
                    if (gettingAnswer)
                    {
                        GetMysteryAnswer();
                    }
                    GetResponseDeductionMode();
                } 
                else 
                {
                    if (tutorialMode)
                    {
                        GetTutorialAnswer();
                    } else
                    {
                        GetResponse();
                    }
                }
            }
        }
    }

    public IEnumerator EnterDialogueMode(List<ChatMessage> thisRouterMessages, List<ChatMessage> thisConversationMessages, List<string> thisLastTurns, 
                                         TextAsset inkJSON, TextAsset messagesJSON, GameObject thisNpc, bool thisShouldDestroy, string name, int numTopics, 
                                         bool thisHasOutro, int thisFlagNumber, bool thisDeductionMode, 
                                         string thisKnotName = "")
    {
        yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = true;
        currentStory = new Story(inkJSON.text);
        currentMessages = new Story(messagesJSON.text);
        routerMessages = thisRouterMessages;
        conversationMessages = thisConversationMessages;
        lastTurns = new Queue<string>(thisLastTurns);
        knotName = thisKnotName;
        maxTopics = numTopics;
        hasOutro = thisHasOutro;
        flagNumber = thisFlagNumber;
        shouldDestroy = thisShouldDestroy;
        npc = thisNpc;

        if (thisDeductionMode)
        {
            convinced = false;
        }
        deductionMode = thisDeductionMode;
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
    private void PlayDialogueSound(int currentDisplayedCharacterCount)
    {
        if (currentDisplayedCharacterCount % frequency == 0) 
        {
            audioSource.PlayOneShot(dialogueTypingSoundClip);
        }
    }

    IEnumerator TypeSentence(string sentence) 
    {
        canContinueToNextLine = false;
        dialogueText.text = sentence;
        dialogueText.maxVisibleCharacters = 0;
        foreach (char letter in sentence.ToCharArray()) {
            if (submitButtonPressedThisFrame) {
                submitButtonPressedThisFrame = false;
                dialogueText.maxVisibleCharacters = sentence.Length;
                break;
            }
            if (letter == '<' || isAddingRichTextTag) {
                isAddingRichTextTag = true;
                if (letter == '>') {
                    isAddingRichTextTag = false;
                }
            }
            else {
                PlayDialogueSound(dialogueText.maxVisibleCharacters);
                dialogueText.maxVisibleCharacters ++;
                yield return new WaitForSeconds(textSpeed);
            }
        }
        canContinueToNextLine = true;
    }
    public void EndConversation() 
    {
        // Debug.Log("Entered EndConversation");
        // in case we need to
        prewrittenMode = true;
        playerTurn = false;
        inputFieldObject.SetActive(false);
        inputField.text = "";
        if (hasOutro && !outroPlayed)
        {
            // this is pretty hacky lol
            if (deductionMode && gettingAnswer)
            {
                // we do a little hardcoding
                currentStory.ChoosePathString("yes_suicide");
            }
            else if (deductionMode && !convinced)
            {
                currentStory.ChoosePathString("bad_outro");
            }
            else 
            {
                currentStory.ChoosePathString("outro");
            }
            currentSentence = currentStory.Continue();
            // Debug.Log("CURRENT SENTENCE:" +  currentSentence);
            HandleTags(currentStory.currentTags);
            StartCoroutine(TypeSentence(currentSentence));
            outroPlayed = true;
        } else 
        {
            // StartCoroutine(ExitDialogueMode());
            ExitDialogueMode();
        }
    }

    private void ExitDialogueModeButton()
    {
        dialogueIsPlaying = false;
        explored_topics = new HashSet<string>();
        playerTurn = false;
        outroPlayed = false;
        dialoguePanel.SetActive(false);
        MainManager.Instance.SetFlagStatus(flagNumber, true);
        prewrittenMode = true;
        response.text = "";
        if (shouldDestroy)
        {
            Destroy(npc);
        }
    }

    private void ExitDialogueMode()
    {
        // a little hardcoding
        if (deductionMode)
        {
            LevelLoader.GetInstance().LoadSelected("Ending");
        }
        // yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = false;
        explored_topics = new HashSet<string>();
        playerTurn = false;
        outroPlayed = false;
        dialoguePanel.SetActive(false);
        MainManager.Instance.SetFlagStatus(flagNumber, true);
        prewrittenMode = true;
        response.text = "";
        if (shouldDestroy)
        {
            Destroy(npc);
        }
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
                    // TODO: need to make a "do not add" tag for stuff that we shouldn't add to the last turns
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
                    // StartCoroutine(ExitDialogueMode());
                    ExitDialogueMode();
                }
                else if (deductionMode && explored_topics.Contains("reason_1") || deductionMode && explored_topics.Count == maxTopics)
                {
                    convinced = true;
                    MainManager.Instance.mysterySolved = true;
                    EndConversation();
                }
                else if (explored_topics.Count == maxTopics)
                {
                    EndConversation();
                } 
                else 
                {
                    if (tutorialMode)
                    {
                        StartCoroutine(TooltipScript.Instance.openTooltip("You're the detective! Type anything you want in the dialogue box. For instance, try asking how August died.", 7));
                    }
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
        if (sentence == "") {
            Debug.Log("Nothing to update");
            return;
        }
        string tag = speaker == ERIKA ? "E: " : "N: ";
        string turn = tag + sentence;
        if (lastTurns.Count >= 3)
        {
            lastTurns.Dequeue();
        }
        lastTurns.Enqueue(turn);
        // Debug.Log("LAST TURNS:" + QueueToString(lastTurns));
    }
    public async void GetMysteryAnswer()
    {
        if (inputField.text.Length < 1)
        {
            return;
        }
        speakerNameText.text = npcName;
        string userInput = inputField.text;
        inputField.text = "";
        inputFieldObject.SetActive(false);
        response.text = "(thinking)";
        ChatMessage routerInput = new ChatMessage()
        {
            Role = "user",
            Content = userInput
        };
        routerMessages.Add(routerInput);
        var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-4-1106-preview", 
            Messages = routerMessages,
            Temperature = 0.0f,
        });
        routerMessages.RemoveAt(routerMessages.Count - 1); // pop so as not to polute the context window
        string slot = completionResponse.Choices[0].Message.Content;
        // Debug.Log("SLOT:" + slot);
        if (slot == "none")
        {
            currentSentence = "I'm sorry, but that doesn't really answer the question. I'll ask again: did August Laurier commit suicide?";
        } 
        else 
        {
            prewrittenMode = true;
            currentStory.ChoosePathString(slot);
            // if we should continue, overwrite the router messages
            if (slot == "no_suicide")
            {
                UpdateLastTurns(ERIKA, "No. He was killed.");
                routerMessages = new List<ChatMessage>();
                PopulateMessageList(routerMessages, "router_2");
                // Debug.Log(routerMessages[0].Content);
                gettingAnswer = false;
            } else 
            {
                // otherwise, end the conversation with the bad ending
                EndConversation();
            }
        }
        playerTurn = false;
        ContinueStory();
    }

    public async void GetTutorialAnswer ()
    {
        // TODO: if a help text object exists, display it.
        if (inputField.text.Length < 1)
        {
            return;
        }
        speakerNameText.text = npcName;
        string userInput = inputField.text;
        inputField.text = "";
        inputFieldObject.SetActive(false);
        response.text = "(thinking)";
        ChatMessage routerInput = new ChatMessage()
        {
            Role = "user",
            Content = userInput
        };
        routerMessages.Add(routerInput);
        var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-4-1106-preview", 
            Messages = routerMessages,
            Temperature = 0.0f,
        });
        routerMessages.RemoveAt(routerMessages.Count - 1); // pop so as not to pollute the context window
        string slot = completionResponse.Choices[0].Message.Content;
        Debug.Log("SLOT:" + slot);
        prewrittenMode = true;
        currentStory.ChoosePathString(slot);
        if (slot == "topic_0")
        {
            UpdateLastTurns(ERIKA, userInput);
            routerMessages = new List<ChatMessage>();
            PopulateMessageList(routerMessages, "router_2");
            tutorialMode = false;
        }
        playerTurn = false;
        ContinueStory();
    }
    // create another method for tutorial mode
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
        response.text = "(thinking)";
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
            Model = "gpt-4-1106-preview", // use GPT-4 for router bc it's more powerful
            Messages = routerMessages,
            Temperature = 0.0f,
        });
        routerMessages.RemoveAt(routerMessages.Count - 1); // pop so as not to polute the context window
        string slot = completionResponse.Choices[0].Message.Content;
        // Debug.Log("SLOT: " + slot);
        if (slot != "none")
        {
            // step 2: if the user input matches a prewritten response, add the correct response to messageList and 
            // play the prewritten conversation
            // at this step, add the correct input to the set
            if (slot[..5] != "bonus") 
            {
                explored_topics.Add(slot);
            }
            prewrittenMode = true;
            string currTopic = (knotName != "") ? knotName + "." + slot : slot;
            currentStory.ChoosePathString(currTopic);
            PopulateMessageList(conversationMessages, currTopic);
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
        playerTurn = false;
        ContinueStory();
    }

    public async void GetResponseDeductionMode()
    {
        if (inputField.text.Length < 1)
        {
            return;
        }
        speakerNameText.text = npcName;
        string userInput = inputField.text;
        inputField.text = "";
        inputFieldObject.SetActive(false);
        response.text = "(thinking)";

        string routerInputContent = QueueToString(lastTurns) + "\n" + userInput;
        // Debug.Log("CONTENT: " + routerInputContent);

        ChatMessage routerInput = new ChatMessage()
        {
            Role = "user",
            Content = routerInputContent
        };
        routerMessages.Add(routerInput);
        var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-3.5-turbo", 
            Messages = routerMessages,
            Temperature = 0.0f,
        });
        routerMessages.RemoveAt(routerMessages.Count - 1); // pop so as not to polute the context window
        string routed = completionResponse.Choices[0].Message.Content;
        // Debug.Log("ROUTED MESSAGE: " + routed);
        if (routed.Length >= 6 && (routed[..6] == "Reason" || routed[..11] == "Red Herring"))
        {
            string slot = routed.Replace(' ', '_').ToLower();
            // Debug.Log("SLOT: " + slot);
            if (slot.Length < 11) // if it's not red_herring
            {
                explored_topics.Add(slot);
            }
            prewrittenMode = true;
            currentStory.ChoosePathString(slot);
            PopulateMessageList(conversationMessages, slot);
        }
        else 
        {
            // For the deduction presentations, if the reason kind of matches,
            // we just have Erika say it
            UpdateLastTurns(ERIKA, userInput);
            ChatMessage convoInput;
            if (routed == "[CLUE]" || routed == "[CONTRADICTION]")
            {
                convoInput = new ChatMessage()
                {
                    Role = "user",
                    Content = routed + userInput
                };
            }
            else
            {
                convoInput = new ChatMessage()
                {
                    Role = "user",
                    Content = userInput
                };
            }
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
                // if LLM doesn't listen to me
                if (currentSentence[0] == '[')
                {
                    int idx = currentSentence.IndexOf(']');
                    currentSentence = currentSentence.Substring(idx + 2);
                }
                UpdateLastTurns(NPC, currentSentence);
            }
            else
            {
                currentSentence = "Sorry, could you say that again?";
            }
            speakerNameText.text = npcName;
        }
        playerTurn = false;
        ContinueStory();
    }

    private void NPCTurn()
    {
        // response.text = currentSentence;
        // Debug.Log(currentSentence);
        StartCoroutine(TypeSentence(currentSentence));
    }

    private void PopulateMessageList(List<ChatMessage> messageList, string knotName)
    {
        currentMessages.ChoosePathString(knotName);
        while (currentMessages.canContinue)
        {
            string content = currentMessages.Continue();
            string role = currentMessages.currentTags[0];
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

    private void HandleTags(List<string> currentTags)
    {
        // TODO: create (should have popup) and (don't populate messages) tags
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
                case TOOLTIP_TAG:
                    StartCoroutine(TooltipScript.Instance.openTooltip(tagValue, 10));
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

