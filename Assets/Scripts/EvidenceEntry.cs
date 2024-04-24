using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvidenceEntry : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool hasDetails = false;
    [SerializeField] private GameObject detailsIndicator;
    [SerializeField] private Button detailsButton;
    [SerializeField] private GameObject details;
    void Start()
    {
        if (!hasDetails)
        {
            detailsIndicator.SetActive(false);
            detailsButton.enabled = false; // no button but no color change
        }
        else 
        {
            detailsButton.onClick.AddListener(OpenDetails);
        }
    }

    private void OpenDetails()
    {
        details.SetActive(true);
    }
}
