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

    public void onClickAction()
    {
        if (!DialogueManager.GetInstance().dialogueIsPlaying)
        {
            StartCoroutine(DialogueManager.GetInstance().EnterDialogueMode(inkJSON, knotName));
        }
    }
}
