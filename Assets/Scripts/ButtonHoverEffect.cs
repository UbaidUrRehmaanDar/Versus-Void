using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    public string displayName;
    public TextMeshProUGUI targetText;
    public Image borderImage;
    public Image portraitDisplay; // Drag LargePortraitDisplay here
    public Sprite characterPortrait; // Drag the large portrait sprite here

    [Header("Settings")]
    public Color hoverColor = Color.red;
    private Color originalColor;
    private bool isSelected = false;

    void Start()
    {
        if (borderImage != null) originalColor = borderImage.color;
    }

    void Update()
    {
        // If the player presses Escape, deselect this character
        if (Input.GetKeyDown(KeyCode.Escape) && isSelected)
        {
            Deselect();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetText != null) targetText.text = displayName;

        // Show the portrait
        if (portraitDisplay != null && characterPortrait != null)
        {
            portraitDisplay.sprite = characterPortrait;
            portraitDisplay.gameObject.SetActive(true);
        }

        if (borderImage != null) borderImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Only clear text and portrait if NOT selected
        if (!isSelected)
        {
            if (targetText != null) targetText.text = "Waiting  ...";
            if (portraitDisplay != null) portraitDisplay.gameObject.SetActive(false);
            if (borderImage != null) borderImage.color = originalColor;
        }
    }

    public void SelectThisButton()
    {
        isSelected = true;
        if (borderImage != null)
        {
            borderImage.color = hoverColor; // Force it to stay red
        }
    }

    private void Deselect()
    {
        isSelected = false;
        if (targetText != null) targetText.text = "Waiting for Player...";
        if (portraitDisplay != null) portraitDisplay.gameObject.SetActive(false);
        if (borderImage != null) borderImage.color = originalColor;
    }
}