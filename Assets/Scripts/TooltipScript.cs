using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipScript : MonoBehaviour
{
    [SerializeField] private GameObject tooltip;
    [SerializeField] private Button closeTooltipButton;
    [SerializeField] private TextMeshProUGUI TooltipText;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip ding;

    public static TooltipScript Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        tooltip.SetActive(false);
    }

    public IEnumerator openTooltip(string text, int seconds=3)
    {
        TooltipText.text = text;
        tooltip.SetActive(true);
        audioSource.PlayOneShot(ding);
        yield return new WaitForSeconds(seconds);
        tooltip.SetActive(false);
    }
}
