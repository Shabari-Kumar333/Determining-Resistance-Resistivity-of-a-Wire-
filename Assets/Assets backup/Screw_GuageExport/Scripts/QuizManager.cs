using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance;

    [Header("Correct Answer Name")]
    public string correctAnswer = "Screw gauge";

    [Header("UI")]
    public GameObject correctPanel;
    public GameObject wrongPanel;
    public TMP_Text wrongSelectedText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctSFX;
    public AudioClip wrongSFX;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckAnswer(string selected)
    {
        // Reset UI
        correctPanel.SetActive(false);
        wrongPanel.SetActive(false);

        // Normalize both sides
        string selectedClean = selected.Trim().ToLower();
        string correctClean = correctAnswer.Trim().ToLower();

        if (selectedClean == correctClean)
        {
            correctPanel.SetActive(true);
            audioSource.PlayOneShot(correctSFX);
        }
        else
        {
            wrongPanel.SetActive(true);
            wrongSelectedText.text = "You selected: " + selected;
            audioSource.PlayOneShot(wrongSFX);
        }
    }

}
