using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    private const int flagArrayLen = 10;
    [SerializeField] private TextAsset globalsInkJSON;
    public Story globals;

    // In a fully realized game, this would be a list of bools. But for now, this will suffice.
    public bool mysterySolved = false;

    public string lastScene = "";

    [Header("Event Flags")]

    [SerializeField] private bool[] eventFlags = new bool[flagArrayLen];

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // we can call MainManager.Instance now
        Instance = this;
        DontDestroyOnLoad(gameObject);
        globals = new Story(globalsInkJSON.text);

        for (int i = 0; i < flagArrayLen; i++)
        {
            eventFlags[i] = false;
        }
    }

    public bool EventFlagTriggered(int index)
    {
        if (index == -1)
        {
            // this means that there isn't an event flag assigned to this object
            Debug.Log("No event flag assigned to this object.");
            return false;
        }
        return eventFlags[index];
    }

    public void SetFlagStatus(int index, bool status)
    {
         if (index == -1)
        {
            // this means that there isn't an event flag assigned to this object
            return;
        }
        eventFlags[index] = status;
        // not general, but it's okay at this point
        if (index == 3) // IntroDialogueConcluded
        {
            // StartCoroutine(TooltipScript.Instance.openTooltip("Tip: move with the arrow or WASD keys."));
            StartCoroutine(NotebookScript.Instance.AddQuest(0)); // Adrian's Intel
        }
        if (index == 1) // TalkedToAdrian
        {
            // make items clickable (no action needed here)
            StartCoroutine(NotebookScript.Instance.AddQuest(1)); // Suicide? Or Something Else?
            NotebookScript.Instance.CompleteQuest(0);
        }
        if (index == 2) // TalkedToJulian
        {
            Debug.Log("Set Julian flag");
            globals.variablesState["talked_to_julian"] = true;
        }
    }
}
