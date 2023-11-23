using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotebookScript : MonoBehaviour
{
    [SerializeField] private Button openNotebookButton;
    [SerializeField] private Button closeNotebookButton;
    [SerializeField] private Button notesButton;
    [SerializeField] private Button profilesButton;
    [SerializeField] private GameObject notebook;
    [SerializeField] private GameObject profiles;
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
        notes.SetActive(true);
    }

    private void OpenProfiles()
    {
        notes.SetActive(false);
        profiles.SetActive(true);
    }
}
