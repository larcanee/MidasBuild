/**
AUTHOR: Dillon Evans

DESCRIPTION:
A status bar for displaying things like health or mana.

HOW TO USE:
1. Attach the StatusBar component to an object.
2. Set either the health or mana that the status bar is for.
3. Set the bar and/or text objects.
4. Adjust the settings in the Unity editor.

The bar and text objects are expected to be children of a canvas.
**/


using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StatusBar : MonoBehaviour
{
    [Header("Target")]
    [Tooltip(
        "The health that will be used for the status bar.\n" +
        "Either health or mana should be null."
    )]
    public Health health = null;
    [Tooltip(
        "Whether or not the status bar is for shield or health.\n" +
        "Only considered if health is not null."
    )]
    public bool useShield = false;
    [Tooltip(
        "The mana that will be used for the status bar.\n" +
        "Either health or mana should be null."
    )]
    public Mana mana = null;


    [Header("Bar")]
    [Tooltip(
        "The image object to be the status bar.\n" +
        "Will not display a status bar if this is null."
    )]
    public Image bar = null;
    [Tooltip("Whether or not to update the color of the status bar.")]
    public bool updateColor = true;
    [Tooltip("Color of the status bar at 0%.")]
    public Color barMinimumColor = Color.red;
    [Tooltip("Color of the status bar at 50%.")]
    public Color barMedianColor = Color.yellow;
    [Tooltip("Color of the status bar at 100%.")]
    public Color barMaximumColor = Color.green;


    [Header("Text")]
    [Tooltip(
        "The text object to display status text.\n" +
        "Will not display status text if this is null."
    )]
    public TextMeshProUGUI text = null;
    [Tooltip(
        "Format of status text (for string.Format).\n" +
        "{0} is current value.\n" +
        "{1} is maximum value.\n" +
        "{2} is percentage (a float from 0 to 100)."
    )]
    public string textFormat = "{0}/{1} ({2:0}%)";


    [HideInInspector]
    public int currentValue = 0;
    [HideInInspector]
    public int maximumValue = 1;
    [HideInInspector]
    public float valuePercent = 0f;


    private float initialBarWidth = 0f;
    private float initialBarX = 0f;


    private void Start()
    {
        if (bar)
        {
            RectTransform rt = bar.rectTransform;
            initialBarWidth = rt.sizeDelta.x;
            initialBarX = rt.anchoredPosition.x;
        }
    }

    private void Update()
    {
        UpdateValue();
        UpdateBar();
        UpdateText();
    }


    private void UpdateValue()
    {
        if (health)
        {
            if (useShield)
            {
                currentValue = health.shield;
                maximumValue = health.maxShield;
            }
            else
            {
                currentValue = health.health;
                maximumValue = health.maxHealth;
            }
        }
        else if (mana)
        {
            currentValue = mana.mana;
            maximumValue = mana.maxMana;
        }
        else
        {
            currentValue = 0;
        }
        valuePercent = (float) currentValue / maximumValue;
    }

    private void UpdateBar()
    {
        if (!bar) return;
        RectTransform rt = bar.rectTransform;
        // Update size.
        Vector2 size = rt.sizeDelta;
        size.x = initialBarWidth * valuePercent;
        rt.sizeDelta = size;
        // Update position.
        Vector2 position = rt.anchoredPosition;
        float widthDelta = initialBarWidth - size.x;
        position.x = initialBarX - widthDelta / 2f;
        rt.anchoredPosition = position;
        // Update color.
        if (!updateColor) return;
        if (valuePercent <= 0.5f)
        {
            bar.color = Color.Lerp(barMinimumColor, barMedianColor, valuePercent * 2f);
        }
        else
        {
            bar.color = Color.Lerp(barMedianColor, barMaximumColor, valuePercent * 2f - 1f);
        }
    }

    private void UpdateText()
    {
        if (!text) return;
        text.text = string.Format(
            textFormat,
            currentValue, maximumValue, valuePercent * 100f
        );
    }
}
