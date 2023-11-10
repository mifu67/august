using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickSceneTrigger : MonoBehaviour, IClicked
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("The Ink knot to load")]
    [SerializeField] private string knotName;
    
    [Header("Scene to Load")]
    [SerializeField] private string sceneToLoad;
    public void onClickAction()
    {
        if (!inkJSON)
        {
            SceneManager.LoadScene(sceneToLoad);
        } else 
        {
            if (!DialogueManager.GetInstance().dialogueIsPlaying)
            {
                StartCoroutine(DialogueManager.GetInstance().EnterDialogueMode(inkJSON, knotName, sceneToLoad));
            }
        }
        
    }
}
