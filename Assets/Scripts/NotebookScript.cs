using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// add the evidence UI to this class
public class NotebookScript : MonoBehaviour
{
    [SerializeField] private Button openNotebookButton;
    [SerializeField] private Button closeNotebookButton;
    [SerializeField] private Button notesButton;
    [SerializeField] private Button profilesButton;
    [SerializeField] private Button evidenceButton;
    [SerializeField] private Button questsButton;
    [SerializeField] private GameObject notebook;
    [SerializeField] private GameObject profiles;
    [SerializeField] private GameObject evidence;
    [SerializeField] private GameObject quests;
    [SerializeField] private GameObject notes;

    private List<GameObject> tabs = new List<GameObject>();
    public static NotebookScript Instance;
    private bool notebookOpen = false;

    // I'm thinking that we can just hardcode some indices
    // Evidence variables here
    [SerializeField] private List<GameObject> discoveredEvidence = new List<GameObject>();
    [SerializeField] private List<GameObject> questList = new List<GameObject>();
    [SerializeField] private GameObject emptyEvidence;
    [SerializeField] private GameObject emptyQuest;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        emptyEvidence.SetActive(true);
        foreach (GameObject clue in discoveredEvidence)
        {
            clue.SetActive(false);
        }

        emptyQuest.SetActive(true);
        foreach (GameObject quest in questList)
        {
            quest.SetActive(false);
        }

        tabs.Add(notes);
        tabs.Add(profiles);
        tabs.Add(evidence);
        tabs.Add(quests);
    }

    private void Start()
    {
        notebook.SetActive(false);
        openNotebookButton.onClick.AddListener(OpenNotebook);
        closeNotebookButton.onClick.AddListener(CloseNotebook);
        notesButton.onClick.AddListener(OpenNotes);
        profilesButton.onClick.AddListener(OpenProfiles);
        evidenceButton.onClick.AddListener(OpenEvidence);
        questsButton.onClick.AddListener(OpenQuests);
    }

    public bool getNotebookOpen()
    {
        return notebookOpen;
    }
    private void OpenNotebook()
    {
        notebook.SetActive(true);
        notebookOpen = true;
    }

    private void CloseNotebook()
    {
        notebook.SetActive(false);
        notebookOpen = false;
    }

    private void OpenNotes()
    {
        notes.SetActive(true);
        foreach (GameObject tab in tabs) {
            if (tab != notes)
            {
                tab.SetActive(false); 
            }
        }
    }

    private void OpenProfiles()
    {
        profiles.SetActive(true);
        foreach (GameObject tab in tabs) {
            if (tab != profiles)
            {
                tab.SetActive(false); 
            }
        }
    }

    private void OpenEvidence()
    {
        evidence.SetActive(true);
        foreach (GameObject tab in tabs) {
            if (tab != evidence)
            {
                tab.SetActive(false); 
            }
        }
    }

    private void OpenQuests()
    {
        quests.SetActive(true);
        foreach (GameObject tab in tabs) {
            if (tab != quests)
            {
                tab.SetActive(false); 
            }
        }
    }

    public void AddEvidence(int index)
    {
        if (index == -1)
        {
            return;
        }
        
        if (emptyEvidence.activeSelf) {
            emptyEvidence.SetActive(false);
        }
        if (!discoveredEvidence[index].activeSelf)
        {
            discoveredEvidence[index].SetActive(true);
        }
    }

    // change to iEnumerator later
    public void AddQuest(int index) 
    {
        if (emptyQuest.activeSelf) {
            emptyQuest.SetActive(false);
        }
        questList[index].SetActive(true);
        // surface the new quest indicator for 3 seconds
    }

    public void CompleteQuest(int index)
    {
        Debug.Log("Quest completed."); // placeholder
        // surface an indicator of quest completion
        // make COMPLETED text visible
    }
}
