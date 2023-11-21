using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    private static LevelLoader instance;
    // Start is called before the first frame update
    private void Awake()
    {

        if (instance != null)
        {
            Debug.Log("Found more than one LevelLoader in the scene.");
            Destroy(gameObject);
            return;
        }
        // DontDestroyOnLoad(gameObject);
        instance = this;
        }
    public static LevelLoader GetInstance()
    {
        return instance;
    }

    public void LoadSelected(string sceneToLoad)
    {
        // Debug.Log("Loading scene");
        StartCoroutine(LoadSelectedCoroutine(sceneToLoad));
    }
    IEnumerator LoadSelectedCoroutine(string sceneToLoad)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}
