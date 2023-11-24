using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleEndingDialogue : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    private void Update(){
        if (InputManager.GetInstance().GetInteractPressed() && !DialogueManager.GetInstance().dialogueIsPlaying)
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
}
