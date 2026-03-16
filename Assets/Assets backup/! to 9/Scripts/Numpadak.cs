using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class numpadak : MonoBehaviour
{
    [Header("References")]
    public ObservationTable3 observationTable;

    [Header("UI")]
    public GameObject autoFillButton;
    public int maxLimit = 8;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip keyPressSFX, backspaceSFX, autoFillSFX, checkCorrectSFX, checkWrongSFX;

    private TextMeshProUGUI activeText;
    private Dictionary<TextMeshProUGUI, int> wrongCountMap = new Dictionary<TextMeshProUGUI, int>();
    private HashSet<TextMeshProUGUI> lockedTexts = new HashSet<TextMeshProUGUI>();
    private Dictionary<TextMeshProUGUI, string> autoFillValues = new Dictionary<TextMeshProUGUI, string>();

    public void SetActiveText(TextMeshProUGUI text)
    {
        // 1. Basic Checks
        if (text == null || lockedTexts.Contains(text)) return;

        // 2. Strict Check: Ask Table if this click is allowed
        if (observationTable != null)
        {
            if (!observationTable.IsCellAllowed(text)) return;
        }

        activeText = text;

        // Clear text for fresh entry (optional, feels cleaner)
        if (!lockedTexts.Contains(text)) activeText.text = "";

        if (!wrongCountMap.ContainsKey(text)) wrongCountMap[text] = 0;
        if (autoFillButton) autoFillButton.SetActive(wrongCountMap[text] >= 2);

        Debug.Log($"<color=green>Selected:</color> {text.name}");
    }

    public void AddCharacter(string number)
    {
        if (activeText == null) return;
        if (activeText.text.Length >= maxLimit) return;
        activeText.text += number;
        PlaySFX(keyPressSFX);
    }

    public void Backspace()
    {
        if (activeText == null) return;
        if (activeText.text.Length == 0) return;
        activeText.text = activeText.text.Substring(0, activeText.text.Length - 1);
        PlaySFX(backspaceSFX);
    }

    public bool ProcessCheck(string expectedAnswer)
    {
        if (activeText == null) return false;

        bool correct = false;
        if (double.TryParse(activeText.text, out double userVal) && double.TryParse(expectedAnswer, out double expVal))
        {
            correct = Mathf.Approximately((float)userVal, (float)expVal);
        }
        else
        {
            correct = (activeText.text.Trim() == expectedAnswer.Trim());
        }

        if (correct)
        {
            PlaySFX(checkCorrectSFX);
            lockedTexts.Add(activeText);

            // Disconnect immediately. User must manually click the next box (even the Mean).
            activeText = null;

            if (autoFillButton) autoFillButton.SetActive(false);
            return true;
        }
        else
        {
            PlaySFX(checkWrongSFX);
            wrongCountMap[activeText]++;
            autoFillValues[activeText] = expectedAnswer;
            if (wrongCountMap[activeText] >= 2 && autoFillButton) autoFillButton.SetActive(true);
            return false;
        }
    }

    public void AutoFill()
    {
        if (activeText == null || !autoFillValues.ContainsKey(activeText)) return;
        activeText.text = autoFillValues[activeText];
        if (observationTable != null) observationTable.OnCheckButton();
    }

    private void PlaySFX(AudioClip clip) { if (audioSource && clip) audioSource.PlayOneShot(clip); }
    public TextMeshProUGUI GetActiveText() => activeText;
}