using UnityEngine;
using UnityEngine.UI;

public class CalculatorSlideGate : MonoBehaviour
{
    [Header("REFERENCES")]
    public GlobalSlideNavigation slideNavigation;
    public Button calculatorButton;
    public GameObject calculatorPanel;

    [Header("CALCULATOR ALLOWED SLIDES (1–81)")]
    public bool[] calculatorAllowed = new bool[81];

    int lastSlide = -1;

    void Start()
    {
        if (calculatorPanel)
            calculatorPanel.SetActive(false);

        UpdateCalculatorAccess();
    }

    void Update()
    {
        if (slideNavigation.currentSlide != lastSlide)
        {
            lastSlide = slideNavigation.currentSlide;
            UpdateCalculatorAccess();
        }
    }

    void UpdateCalculatorAccess()
    {
        if (!slideNavigation || !calculatorButton) return;

        int slideIndex = slideNavigation.currentSlide - 1;

        bool allow = false;

        if (slideIndex >= 0 && slideIndex < calculatorAllowed.Length)
            allow = calculatorAllowed[slideIndex];

        calculatorButton.interactable = allow;

        if (!allow && calculatorPanel.activeSelf)
            calculatorPanel.SetActive(false);
    }

    public void ToggleCalculator()
    {
        if (!calculatorButton.interactable) return;

        calculatorPanel.SetActive(!calculatorPanel.activeSelf);
    }

}
