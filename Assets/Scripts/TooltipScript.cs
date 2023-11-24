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

    public static TooltipScript Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        tooltip.SetActive(false);
        closeTooltipButton.onClick.AddListener(CloseTooltip);
    }

    public void openTooltip(string text)
    {
        TooltipText.text = text;
        tooltip.SetActive(true);
    }
    private void CloseTooltip()
    {
        tooltip.SetActive(false);
    }
}
