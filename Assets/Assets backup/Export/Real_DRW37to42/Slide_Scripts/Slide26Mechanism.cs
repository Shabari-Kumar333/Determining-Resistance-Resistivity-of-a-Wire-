using UnityEngine;
using TMPro;

public class slide26mechanism : MonoBehaviour
{
    [Header("--- Numpad UI Connections ---")]
    public TMP_Text inputDisplay;       // The text showing the typed number (e.g., "10")
    public GameObject correctObject;    // The Green Check object
    public GameObject wrongObject;      // The Red X object

    [Header("--- Converter UI Connections ---")]
    public TMP_Text meterResultDisplay; // The text showing the result (e.g., "0.1 m")

    [Header("--- Settings ---")]
    public string correctCode = "10";   // The answer we are looking for
    public int maxLimit = 5;            // Max characters user can type

    // Internal variable to store what the user typed
    private string currentInput = "";

    void Start()
    {
        // 1. Reset input variables
        currentInput = "";

        // 2. Update visuals (clears text and sets meters to 0)
        UpdateDisplayAndConversion();

        // 3. Hide validation icons
        ResetFeedback();
    }

    // -------------------------
    // NUMPAD BUTTON FUNCTIONS
    // -------------------------

    // Connect this to Number Buttons (0-9)
    public void AddCharacter(string number)
    {
        ResetFeedback(); // Hide X or Check when typing new numbers

        if (currentInput.Length < maxLimit)
        {
            currentInput += number;
            UpdateDisplayAndConversion(); // Updates both the typing area and the meter result
        }
    }

    // Connect this to Backspace Button
    public void Backspace()
    {
        ResetFeedback();

        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            UpdateDisplayAndConversion();
        }
    }

    // Connect this to "Autofill" Button
    public void AutoFill()
    {
        ResetFeedback();

        // set exactly to 10
        currentInput = "10";

        // update text and calculate meters immediately
        UpdateDisplayAndConversion();

        // Show the green check
        ValidateAnswer();
    }

    // Connect this to "Check" or "Enter" Button
    public void ValidateAnswer()
    {
        // Compare input
        if (currentInput.Trim() == correctCode)
        {
            if (correctObject) correctObject.SetActive(true);
            if (wrongObject) wrongObject.SetActive(false);
        }
        else
        {
            if (correctObject) correctObject.SetActive(false);
            if (wrongObject) wrongObject.SetActive(true);
        }
    }

    // -------------------------
    // INTERNAL LOGIC
    // -------------------------

    void ResetFeedback()
    {
        if (correctObject) correctObject.SetActive(false);
        if (wrongObject) wrongObject.SetActive(false);
    }

    // This function handles updating the screen AND doing the math
    void UpdateDisplayAndConversion()
    {
        // 1. Update the Input Text (The Numpad part)
        if (inputDisplay != null)
        {
            inputDisplay.text = currentInput;
        }

        // 2. Calculate and Update Meters (The Converter part)
        if (meterResultDisplay != null)
        {
            // If empty, show 0 m
            if (string.IsNullOrEmpty(currentInput))
            {
                meterResultDisplay.text = "0 m";
                return;
            }

            // Parse text to float and divide by 100
            if (float.TryParse(currentInput, out float cmValue))
            {
                float finalMeter = cmValue / 100f;
                meterResultDisplay.text = finalMeter.ToString("0.##") + " m";
            }
        }
    }
}