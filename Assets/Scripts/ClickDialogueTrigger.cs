using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDialogueTrigger : MonoBehaviour, IClicked
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("The Ink knot to load")]
    [SerializeField] private string knotName;

    [Header("-1 if there is no associated piece of evidence")]
    [SerializeField] private int evidenceIndex = -1;

    public void onClickAction()
    {
        if (!DialogueManager.GetInstance().dialogueIsPlaying)
        {
            StartCoroutine(DialogueManager.GetInstance().EnterDialogueMode(inkJSON, knotName, "", evidenceIndex));
        }
    }
}
