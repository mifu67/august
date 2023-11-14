using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNpcVisibility : MonoBehaviour
{
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
        if (npcs[0].activeSelf)
        {
            // all NPCs have been activated
            return;
        }

        if (MainManager.Instance.EventFlagTriggered(1))
        {
            foreach (GameObject npc in npcs)
            {
                npc.SetActive(true);
            }
        }
    }
}
