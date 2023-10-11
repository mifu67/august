using System.Collections;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{   
    [SerializeField]
    private TextAsset inkFile;

    [SerializeField]
    private TextMeshProUGUI message;
    
    [SerializeField]
    private float textSpeed;
    private bool isAddingRichTextTag = false;
    static Story story;
    // Start is called before the first frame update
    void Start()
    {
        story = new Story(inkFile.text);
        message.text = string.Empty;
        AdvanceDialogue();
    }

    // Dialogue ends
    private void FinishDialogue() {
        Debug.Log("End of Dialogue.");
        SceneManager.LoadScene("TitleScene");
    }

    // Advance through the text 
    void AdvanceDialogue() {
        string currentSentence = story.Continue();
        StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence) 
    {
        message.text = sentence;
        message.maxVisibleCharacters = 0;
        foreach (char letter in sentence.ToCharArray()) {
            if (letter == '<' || isAddingRichTextTag) {
                isAddingRichTextTag = true;
                if (letter == '>') {
                    isAddingRichTextTag = false;
                }
            }
            else {
                message.maxVisibleCharacters ++;
                yield return new WaitForSeconds(textSpeed);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (story.canContinue)
            {
                AdvanceDialogue();
            } else {
                FinishDialogue();
            }
        }
    }
}
