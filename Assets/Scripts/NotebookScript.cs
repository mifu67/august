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
    [SerializeField] private GameObject notebook;
    [SerializeField] private GameObject profiles;
    [SerializeField] private GameObject evidence;
    [SerializeField] private GameObject notes;
    public static NotebookScript Instance;
    private bool notebookOpen = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        notebook.SetActive(false);
        openNotebookButton.onClick.AddListener(OpenNotebook);
        closeNotebookButton.onClick.AddListener(CloseNotebook);
        notesButton.onClick.AddListener(OpenNotes);
        profilesButton.onClick.AddListener(OpenProfiles);
        evidenceButton.onClick.AddListener(OpenEvidence);
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
        profiles.SetActive(false);
        evidence.SetActive(false);
        notes.SetActive(true);
    }

    private void OpenProfiles()
    {
        notes.SetActive(false);
        evidence.SetActive(false);
        profiles.SetActive(true);
    }

    private void OpenEvidence()
    {
        notes.SetActive(false);
        profiles.SetActive(false);
        evidence.SetActive(true);
    }
}
