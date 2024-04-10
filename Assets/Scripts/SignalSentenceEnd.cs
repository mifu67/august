using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalSentenceEnd : MonoBehaviour
{
    [Header("Sprite")]
    [SerializeField] private GameObject sprite;
    
    private void Awake()
    {
        sprite.SetActive(false);
    }

    public void ShowSignal()
    {
        sprite.SetActive(true);
    }

    public void HideSignal()
    {
        sprite.SetActive(false);
    }
}
