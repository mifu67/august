using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMainGame : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LevelLoader.GetInstance().LoadSelected("Home");
        }
    }
}
