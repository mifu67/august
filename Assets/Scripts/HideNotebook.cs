using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideNotebook : MonoBehaviour
{
    private GameObject notebook;
    // Start is called before the first frame update
    void Start()
    {
        notebook = GameObject.Find("NotebookCanvas");
        if (notebook != null)
        {
            notebook.SetActive(false);
        }
    }

}
