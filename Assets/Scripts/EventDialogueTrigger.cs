using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDialogueTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private bool playerInRange;
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Don't load until this is triggered")]
    [SerializeField] int gateFlagNumber = -1;

    [Header("Flag Number")]
    [SerializeField] int flagNumber = -1;

    [Header("Trigger this flag after conversation")]
    [SerializeField] int postConvoFlagNumber = -1;

    [Header("Time to wait before triggering dialogue")]
    [SerializeField] float waitTime = 0f;

    private void Awake()
    {
        if (MainManager.Instance)
        // loading out of order for the intro dialogue sequence
        // if it's our first time loading the scene, we don't need this check anyway
        {
            if (gateFlagNumber != -1 && !MainManager.Instance.EventFlagTriggered(gateFlagNumber))
            {
                gameObject.SetActive(false);
            }
            if (MainManager.Instance.EventFlagTriggered(flagNumber))
            {
                gameObject.SetActive(false);
            }
        }
        playerInRange = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!gameObject.activeSelf && MainManager.Instance.EventFlagTriggered(gateFlagNumber))
        {
            gameObject.SetActive(true);
        }
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {   
            StartCoroutine(WaitAndTalk());
        }
    }

    IEnumerator WaitAndTalk()
    {
        yield return new WaitForSeconds(waitTime);
        DialogueManager.GetInstance().EnterDialogueModeEvent(inkJSON, "", "", postConvoFlagNumber);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
            MainManager.Instance.SetFlagStatus(flagNumber, true);
        }
    }
}
