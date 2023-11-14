using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNpcVisibility : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            return;
        }

        if (MainManager.Instance.EventFlagTriggered(1))
        {
            gameObject.SetActive(true);
        }
    }
}
