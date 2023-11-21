using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNpcVisibility : MonoBehaviour
{
    private bool activated = false;
    [SerializeField] private List<GameObject> npcs = new List<GameObject>();
    private void Awake()
    {
        foreach (GameObject npc in npcs)
        {
            npc.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            return;
        }

        // a little hardcoding
        if (MainManager.Instance.EventFlagTriggered(1))
        {
            foreach (GameObject npc in npcs)
            {
                if (npc == null)
                {
                    continue;
                }
                npc.SetActive(true);
            }
            activated = true;
        }
    }
}
