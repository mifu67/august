using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Details : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button closeButton;
    void Start()
    {
        closeButton.onClick.AddListener(CloseDetails);
    }

    private void CloseDetails() 
    {
        gameObject.SetActive(false);
    }
}
