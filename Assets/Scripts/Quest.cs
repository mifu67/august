using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [SerializeField] private GameObject inProgressLabel;
    [SerializeField] private GameObject completeLabel;
    
    public void MarkQuestCompleted()
    {
        inProgressLabel.SetActive(false);
        completeLabel.SetActive(true);
    }
}
