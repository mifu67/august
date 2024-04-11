using System.Collections;
using UnityEngine;
using Ink.Runtime;
using TMPro;

public class IntroDialogueManager : MonoBehaviour
{   
    [SerializeField]
    private TextAsset inkFile;

    [SerializeField]
    private TextMeshProUGUI message;
    
    [SerializeField]
    private float textSpeed;

    [SerializeField]
    private SignalSentenceEnd signal;

    [SerializeField]
    private AudioClip dialogueTypingSoundClip;

    [SerializeField]
    private AudioClip NextLineSoundClip;
    [Range(1, 5)]

    [SerializeField]
    private int frequency = 2;

    private AudioSource audioSource;
    private bool isAddingRichTextTag = false;
    private bool canContinueToNextLine = false;
    private Coroutine displayLineCoroutine;

    private bool submitButtonPressedThisFrame = false;
    static Story story;
    // Start is called before the first frame update
    void Start()
    {
        story = new Story(inkFile.text);
        message.text = string.Empty;
        AdvanceDialogue();
    }

    private void Awake() {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    // Dialogue ends
    private void FinishDialogue() {
        Debug.Log("End of intro dialogue.");
        LevelLoader.GetInstance().LoadSelected("TitleScene");
    }

    // Advance through the text 
    void AdvanceDialogue() {
        if (displayLineCoroutine != null) 
        {
            StopCoroutine(displayLineCoroutine);
        }
        signal.HideSignal();
        string currentSentence = story.Continue();
        displayLineCoroutine = StartCoroutine(TypeSentence(currentSentence));
    }
    
    IEnumerator TypeSentence(string sentence) 
    {
        yield return new WaitForSeconds(0.6f);
        canContinueToNextLine = false;
        message.text = sentence;
        message.maxVisibleCharacters = 0;
        foreach (char letter in sentence.ToCharArray()) {
            if (submitButtonPressedThisFrame) {
                Debug.Log("Skip to end of line.");
                submitButtonPressedThisFrame = false;
                message.maxVisibleCharacters = sentence.Length;
                break;
            }
            if (letter == '<' || isAddingRichTextTag) {
                isAddingRichTextTag = true;
                if (letter == '>') {
                    isAddingRichTextTag = false;
                }
            }
            else {
                PlayDialogueSound(message.maxVisibleCharacters);
                message.maxVisibleCharacters ++;
                yield return new WaitForSeconds(textSpeed);
            }
        }
        signal.ShowSignal();
        canContinueToNextLine = true;
    }

    private void PlayDialogueSound(int currentDisplayedCharacterCount)
    {
        if (currentDisplayedCharacterCount % frequency == 0) 
        {
            audioSource.PlayOneShot(dialogueTypingSoundClip);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetInstance().GetSubmitPressed())
        {
            submitButtonPressedThisFrame = true;

        }
        if (story.canContinue)
        {
            if (canContinueToNextLine && submitButtonPressedThisFrame) {
                submitButtonPressedThisFrame = false;
                audioSource.PlayOneShot(NextLineSoundClip);
                AdvanceDialogue();
            }
        } else {
            if (submitButtonPressedThisFrame) {
                submitButtonPressedThisFrame = false;
                audioSource.PlayOneShot(NextLineSoundClip);
                FinishDialogue();
            }
        }
    }
}
