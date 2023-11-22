using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleEndingDialogue : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    // Start is called before the first frame update
    void Start()
    {
        if (MainManager.Instance.mysterySolved)
        {
            DialogueManager.GetInstance().EnterDialogueModeEvent(inkJSON, "good_ending", "Feedback");
        }
        else
        {
            DialogueManager.GetInstance().EnterDialogueModeEvent(inkJSON, "bad_ending", "Feedback");
        }
    }
}
