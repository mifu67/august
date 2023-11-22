using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
     
    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalsJSON;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerNameText;

    [SerializeField] private Animator portrait1Animator;
    [SerializeField] private Animator portrait2Animator;

    [SerializeField] private Image sprite1;
    [SerializeField] private Image sprite2;

    [SerializeField]
    private float textSpeed;

    [SerializeField]
    private string sceneToLoad = "";

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }
    private static DialogueManager instance;

    private string speaker1 = "";
    private string speaker2 = "";
    private bool isAddingRichTextTag = false;
    private const string SPEAKER_1_TAG = "speaker1";
    private const string SPEAKER_2_TAG = "speaker2";
    private const string PORTRAIT_1_TAG = "portrait1";

    private const string PORTRAIT_2_TAG = "portrait2";

    private const string SPEAKING_TAG = "speaking";

    private const string COLOR_TAG = "color";

    // private DialogueVariables dialogueVariables;

    private void Awake()
    {

        if (instance != null)
        {
            Debug.Log("Found more than one Dialogue Manager in the scene.");
            return;
        }
        instance = this;
        Debug.Log("Initializing dialogue manager");
        // dialogueVariables = new DialogueVariables(loadGlobalsJSON);
        }
    public static DialogueManager GetInstance()
    {
        return instance;
    }
    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }
        if (InputManager.GetInstance().GetSubmitPressed())
        {
            ContinueStory();
        }
    }
    // no wait times for events
    public void EnterDialogueModeEvent(TextAsset inkJSON, string knotName = "", string thisSceneToLoad = "")
    {
        currentStory = new Story(inkJSON.text);
        if (knotName != "")
        {
            currentStory.ChoosePathString(knotName);
        }
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        sceneToLoad = thisSceneToLoad;

        ContinueStory();
    }

    public IEnumerator EnterDialogueMode(TextAsset inkJSON, string knotName = "", string thisSceneToLoad = "") 
    {
        yield return new WaitForSeconds(0.2f);
        currentStory = new Story(inkJSON.text);
        // some hardcoding here
        if ((bool) MainManager.Instance.globals.variablesState["talked_to_julian"] && 
        ! (bool) currentStory.variablesState["talked_to_julian"])
        {
            currentStory.variablesState["talked_to_julian"] = true;
        }
        sceneToLoad = thisSceneToLoad;
        if (knotName != "")
        {
            currentStory.ChoosePathString(knotName);
        }
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        if (sceneToLoad != "")
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        yield return new WaitForSeconds(0.2f);
        // dialogueVariables.StopListening(currentStory);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            string currentSentence = currentStory.Continue();
            HandleTags(currentStory.currentTags);
            StartCoroutine(TypeSentence(currentSentence));
        } else 
        {
            StartCoroutine(ExitDialogueMode());
        }
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
                case SPEAKER_1_TAG:
                    // Debug.Log("speaker1=" + tagValue);
                    speaker1 = tagValue;
                    break;
                case SPEAKER_2_TAG:
                    // Debug.Log("speaker2=" + tagValue);
                    speaker2 = tagValue;
                    break;
                case PORTRAIT_1_TAG:
                    // Debug.Log("portrait1=" + tagValue);
                    portrait1Animator.Play(tagValue);
                    break;
                case PORTRAIT_2_TAG:
                    // Debug.Log("portrait2=" + tagValue);
                    portrait2Animator.Play(tagValue);
                    break;
                case SPEAKING_TAG:
                    // Debug.Log("speaking=" + tagValue);
                    if (tagValue == "speaker1")
                    {
                        sprite1.color = Color.white;
                        sprite2.color = Color.gray;
                        speakerNameText.text = speaker1;
                    } else if (tagValue == "speaker2")
                    {
                        sprite1.color = Color.gray;
                        sprite2.color = Color.white;
                        speakerNameText.text = speaker2;
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
