using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadIntro : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LevelLoader.GetInstance().LoadSelected("IntroScene");
    }
}
