using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotebookScript : MonoBehaviour
{
    [SerializeField] private Button openNotebookButton;
    [SerializeField] private Button closeNotebookButton;
    [SerializeField] private GameObject notebook;
    public static NotebookScript Instance;
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
    }

    private void OpenNotebook()
    {
        notebook.SetActive(true);
    }

    private void CloseNotebook()
    {
        notebook.SetActive(false);
    }
}
