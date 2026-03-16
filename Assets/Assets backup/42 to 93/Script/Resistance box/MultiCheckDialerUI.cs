using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MultiCheckDialerUI : MonoBehaviour
{
    [Header("TABLE FIELDS")]
    public List<TMP_InputField> resistanceFields;
    public List<TMP_InputField> lengthFields;
    public List<TMP_InputField> s1Fields;
    public List<TMP_InputField> s2Fields;
    public List<TMP_InputField> meanFields;

    [Header("CORRECT IMAGES")]
    public List<GameObject> resistanceCorrect;
    public List<GameObject> lengthCorrect;
    public List<GameObject> s1Correct;
    public List<GameObject> s2Correct;
    public List<GameObject> meanCorrect;

    [Header("WRONG IMAGES")]
    public List<GameObject> resistanceWrong;
    public List<GameObject> lengthWrong;
    public List<GameObject> s1Wrong;
    public List<GameObject> s2Wrong;
    public List<GameObject> meanWrong;

    [Header("Sources")]
    public ResistanceSystem resistanceBox;
    public S_Rightgap S_Rightgap;

    protected float[] storedS1;
    protected float[] storedS2;
    protected float[] storedMean;

    [Header("UI")]
    public Button checkButton;
    public Button retryButton;

    [Header("TOTAL MEAN")]
    public TMP_InputField totalMeanField;
    public GameObject totalMeanCorrect;
    public GameObject totalMeanWrong;

    [Header("AUDIO")]
    public AudioSource audioSource;
    public AudioClip correctSFX;
    public AudioClip wrongSFX;

    public float TOL = 0.01f;

    protected int currentRow = 0;
    TMP_InputField activeField;
    protected int activeColumn = -1; // 0=R 1=L 2=S1 3=S2 4=Mean

    TMP_InputField lastActiveField;
    GameObject lastCorrectImage;

    Coroutine hideRoutine;

    [Header("AUTO FILL")]
    public Button autoFillButton;
    Dictionary<TMP_InputField, int> wrongCount =
    new Dictionary<TMP_InputField, int>();

    enum Phase { S1, S2, Mean }
    Phase currentPhase = Phase.S1;

    int s1Row = 0;
    int s2Row = 0;



    const int MAX_WRONG = 3;
    // ===============================
    void Start()
    {
        LockAllFieldsInitially();
        int rows = meanFields.Count;

        storedS1 = new float[rows];
        storedS2 = new float[rows];
        storedMean = new float[rows];

        for (int i = 0; i < rows; i++)
        {
            storedS1[i] = float.NaN;
            storedS2[i] = float.NaN;
            storedMean[i] = float.NaN;
        }
        autoFillButton.gameObject.SetActive(false);
        autoFillButton.onClick.AddListener(OnAutoFillPressed);


        totalMeanField.onSelect.AddListener(_ =>
        {
            activeField = totalMeanField;
            activeColumn = -99;
        });

        Register(resistanceFields, 0);
        Register(lengthFields, 1);
        Register(s1Fields, 2);
        Register(s2Fields, 3);
        Register(meanFields, 4);

        checkButton.onClick.AddListener(OnCheckPressed);
        retryButton.onClick.AddListener(OnRetryPress);
        retryButton.gameObject.SetActive(false);
    }

    // ======================= NUMBER PAD =======================
    public void OnNumberPress(string num)
    {
        if (activeField == null || !activeField.interactable) return;
        activeField.text += num;
    }

    public void OnDecimalPress()
    {
        if (activeField == null || !activeField.interactable) return;

        if (!activeField.text.Contains("."))
        {
            if (activeField.text == "")
                activeField.text = "0.";
            else
                activeField.text += ".";
        }
    }

    public void OnClearPress()
    {
        if (activeField == null || !activeField.interactable) return;

        if (activeField.text.Length > 0)
            activeField.text = activeField.text.Substring(0, activeField.text.Length - 1);

        GameObject wrongImg = GetWrongImage();
        if (wrongImg != null)
            wrongImg.SetActive(false);

        retryButton.gameObject.SetActive(false);
    }

    // ===============================
    void Register(List<TMP_InputField> list, int column)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int row = i;
            list[i].onSelect.AddListener((_) =>
            {
                if (!list[row].interactable) return;

                if (lastCorrectImage != null)
                    lastCorrectImage.SetActive(false);

                activeField = list[row];
                activeColumn = column;

                if (column == 2)        // S1
                    s1Row = row;
                else if (column == 3)   // S2
                    s2Row = row;
                else
                    currentRow = row;  // R / L / Mean

                lastActiveField = activeField;
                lastCorrectImage = GetCorrectImage(row, column);
            });
        }
    }


    GameObject GetCorrectImage(int row, int column)
    {
        switch (column)
        {
            case 0: return resistanceCorrect[row];
            case 1: return lengthCorrect[row];
            case 2: return s1Correct[row];
            case 3: return s2Correct[row]; // ✅ CLEAN
            case 4: return meanCorrect[row];
        }
        return null;
    }




    // ===============================
    protected virtual void CheckCurrentField()
    {
        if (activeField == null) return;
        if (currentRow >= resistanceFields.Count) return;

        int row = currentRow;

        float R = resistanceBox.Resistance;
        float L = S_Rightgap.balanceLength;

        float S1 = R * L / (100f - L);
        float S2 = R * (100f - L) / L;

        float S1r = Mathf.Round(S1 * 100f) / 100f;
        float S2r = Mathf.Round(S2 * 100f) / 100f;

        float Mean = Mathf.Round(((S1r + S2r) / 2f) * 1000f) / 1000f;

        bool correct = false;

        switch (activeColumn)
        {
            case 0:
                correct = SafeCheck(resistanceFields, resistanceCorrect, resistanceWrong, row, R);
                break;

            case 1:
                correct = SafeCheck(lengthFields, lengthCorrect, lengthWrong, row, L);
                break;

            case 2:
                correct = SafeCheck(s1Fields, s1Correct, s1Wrong, row, S1);
                if (correct) storedS1[row] = S1r;
                break;

            case 3: // S2
                {
                    int s2Index = s2Row;   // ✅ different variable name

                    correct = SafeCheck(
                        s2Fields,
                        s2Correct,
                        s2Wrong,
                        s2Index,
                        S2
                    );

                    if (correct)
                        storedS2[s2Index] = S2r;

                    break;
                }



            case 4:
                if (float.IsNaN(storedS1[row]) || float.IsNaN(storedS2[row]))
                    return;

                float meanExpected =
                    (float)System.Math.Round(
                        (storedS1[row] + storedS2[row]) / 2f,
                        3,
                        System.MidpointRounding.AwayFromZero
                    );

                correct = SafeCheck(meanFields, meanCorrect, meanWrong, row, meanExpected);
                break;
        }

        if (correct)
            storedMean[row] = Mean;
    }

    public void OnCheckMean()
    {
        activeColumn = 4;
        CheckCurrentField();
        OnCheckPressed();
    }

    // ===============================
    protected bool SafeCheck(List<TMP_InputField> fields,
                             List<GameObject> ok,
                             List<GameObject> bad,
                             int i,
                             float expected)
    {
        if (i >= fields.Count || i >= ok.Count || i >= bad.Count)
            return false;

        return CheckOne(fields[i], ok[i], bad[i], expected);
    }

    bool CheckOne(TMP_InputField field, GameObject ok, GameObject bad, float expected)
    {
        if (!float.TryParse(field.text, out float user))
        {
            RegisterWrong(field);
            bad.SetActive(true);
            ok.SetActive(false);

            PlayWrongSFX();
            RestartHideTimer();
            retryButton.gameObject.SetActive(true);
            return false;
        }

        bool correct = Mathf.Abs(user - expected) <= TOL;

        ok.SetActive(correct);
        bad.SetActive(!correct);

        if (correct)
        {
            field.interactable = false;

            ResetWrong(field);
            autoFillButton.gameObject.SetActive(false);
            PlayCorrectSFX();

            UnlockNextField(); // 🔓 unlock strictly next field
        }


        else
        {
            // ❌ KEEP FIELD EDITABLE ON WRONG
            field.interactable = true;

            RegisterWrong(field);
            PlayWrongSFX();
        }

        RestartHideTimer();
        return correct;
    }

    void RegisterWrong(TMP_InputField field)
    {
        if (!wrongCount.ContainsKey(field))
            wrongCount[field] = 0;

        wrongCount[field]++;

        if (wrongCount[field] >= MAX_WRONG)
            autoFillButton.gameObject.SetActive(true);
    }

    void ResetWrong(TMP_InputField field)
    {
        if (wrongCount.ContainsKey(field))
            wrongCount[field] = 0;
    }
    void OnAutoFillPressed()
    {
        if (activeField == null) return;

        float value = GetExpectedValueForActiveField();
        if (float.IsNaN(value)) return;

        // 1️⃣ Fill correct value
        activeField.text = FormatAutoFillValue(value);

        // 2️⃣ Hide autofill button
        autoFillButton.gameObject.SetActive(false);

        // 3️⃣ Reset wrong counter
        ResetWrong(activeField);

        // 4️⃣ Validate IMMEDIATELY (same as Check button)
        if (activeField == totalMeanField)
        {
            CheckTotalMean();      // ✅ correct image + sound
        }
        else
        {
            CheckCurrentField();  // ✅ correct image + sound + unlock next
        }
    }


    float GetExpectedValueForActiveField()
    {
        if (currentRow < 0) return float.NaN;

        float R = resistanceBox.Resistance;
        float L = S_Rightgap.balanceLength;

        if (L <= 0 || L >= 100) return float.NaN;

        // 🔢 Rounded exactly like CheckCurrentField()
        float S1r = Mathf.Round((R * L / (100f - L)) * 100f) / 100f;
        float S2r = Mathf.Round((R * (100f - L) / L) * 100f) / 100f;

        float Mean = Mathf.Round(((S1r + S2r) / 2f) * 1000f) / 1000f;

        switch (activeColumn)
        {
            case 0: return R;       // Resistance
            case 1: return L;       // Length
            case 2: return S1r;     // S1
            case 3: return S2r;     // S2
            case 4: // Mean
                if (float.IsNaN(storedS1[currentRow]) || float.IsNaN(storedS2[currentRow]))
                    return float.NaN;

                return (float)System.Math.Round(
                    (storedS1[currentRow] + storedS2[currentRow]) / 2f,
                    3,
                    System.MidpointRounding.AwayFromZero
                );

        }

        return float.NaN;
    }

    string FormatAutoFillValue(float value)
    {
        switch (activeColumn)
        {
            case 0: // Resistance R
            case 1: // Length L
                    // Remove trailing zeros
                return value.ToString("0.##");

            case 2: // S1
            case 3: // S2
                return value.ToString("0.00");

            case 4: // Mean
                return value.ToString("0.000");
        }

        return value.ToString();
    }



    float CalculateTotalMean()
    {
        float sum = 0f;
        int count = 0;

        for (int i = 0; i < storedS1.Length; i++)
        {
            if (float.IsNaN(storedS1[i]) || float.IsNaN(storedS2[i]))
                continue;

            sum += (storedS1[i] + storedS2[i]) / 2f;
            count++;
        }

        if (count == 0) return float.NaN;

        return (float)System.Math.Round(
            sum / count,
            3,
            System.MidpointRounding.AwayFromZero
        );
    }

    void CheckTotalMean()
    {
        if (!float.TryParse(totalMeanField.text, out float user))
        {
            totalMeanField.interactable = false; // 🔒 final lock
            totalMeanWrong.SetActive(true);
            totalMeanCorrect.SetActive(false);

            PlayWrongSFX();
            RestartHideTimer();
            return;
        }

        float expected = CalculateTotalMean();
        if (float.IsNaN(expected)) return;

        user = (float)System.Math.Round(user, 3, System.MidpointRounding.AwayFromZero);
        expected = (float)System.Math.Round(expected, 3, System.MidpointRounding.AwayFromZero);

        bool correct = Mathf.Abs(user - expected) <= 0.0001f;

        totalMeanCorrect.SetActive(correct);
        totalMeanWrong.SetActive(!correct);

        if (correct) PlayCorrectSFX();
        else PlayWrongSFX();

        RestartHideTimer();
    }

    public void OnSelectTotalMean()
    {
        if (!totalMeanField.interactable) return;

        if (lastCorrectImage != null)
            lastCorrectImage.SetActive(false);

        activeField = totalMeanField;
        activeColumn = -1;
        currentRow = -1;

        lastActiveField = activeField;
        lastCorrectImage = totalMeanCorrect;
    }

    public void OnCheckPressed()
    {
        if (activeField == totalMeanField)
        {
            CheckTotalMean();
            return;
        }

        CheckCurrentField();
    }

    public void OnRetryPress()
    {
        if (activeField == null) return;

        activeField.text = "";
        activeField.interactable = true;   // ✅ UNLOCKS FIELD
        retryButton.gameObject.SetActive(false);

        GameObject wrongImg = GetWrongImage();
        if (wrongImg != null)
            wrongImg.SetActive(false);
    }


    GameObject GetWrongImage()
    {
        switch (activeColumn)
        {
            case 0: return resistanceWrong[currentRow];
            case 1: return lengthWrong[currentRow];
            case 2: return s1Wrong[s1Row];
            case 3: return s2Wrong[s2Row];
            case 4: return meanWrong[currentRow];
        }
        return null;
    }






    // ======================= AUDIO =======================
    void PlayCorrectSFX()
    {
        if (audioSource && correctSFX)
            audioSource.PlayOneShot(correctSFX);
    }

    void PlayWrongSFX()
    {
        if (audioSource && wrongSFX)
            audioSource.PlayOneShot(wrongSFX);
    }

    // ======================= AUTO HIDE =======================
    void RestartHideTimer()
    {
        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        hideRoutine = StartCoroutine(HideIndicatorsAfterDelay());
    }

    IEnumerator HideIndicatorsAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        HideAllIndicators();
        hideRoutine = null;
    }

    void HideAllIndicators()
    {
        foreach (var g in resistanceCorrect) g.SetActive(false);
        foreach (var g in resistanceWrong) g.SetActive(false);
        foreach (var g in lengthCorrect) g.SetActive(false);
        foreach (var g in lengthWrong) g.SetActive(false);
        foreach (var g in s1Correct) g.SetActive(false);
        foreach (var g in s1Wrong) g.SetActive(false);
        foreach (var g in s2Correct) g.SetActive(false);
        foreach (var g in s2Wrong) g.SetActive(false);
        foreach (var g in meanCorrect) g.SetActive(false);
        foreach (var g in meanWrong) g.SetActive(false);

        if (totalMeanCorrect) totalMeanCorrect.SetActive(false);
        if (totalMeanWrong) totalMeanWrong.SetActive(false);
    }
    void LockAllFieldsInitially()
    {
        foreach (var f in resistanceFields) f.interactable = false;
        foreach (var f in lengthFields) f.interactable = false;
        foreach (var f in s1Fields) f.interactable = false;
        foreach (var f in s2Fields) f.interactable = false;
        foreach (var f in meanFields) f.interactable = false;
        totalMeanField.interactable = false;


        // 🔓 Start ONLY with R1 (index 0)
        resistanceFields[0].interactable = true;
    }

    void UnlockNextField()
    {
        int r = currentRow;

        switch (activeColumn)
        {
            case 0: // R → L
                lengthFields[r].interactable = true;
                break;

            case 1: // L → S1 or S2
                if (r < 5)
                {
                    // S1 phase
                    s1Fields[r].interactable = true;
                }
                else
                {
                    // S2 phase (offset by 5)
                    s2Fields[r - 5].interactable = true;
                }
                break;

            case 2: // S1
                if (r < 4)
                {
                    resistanceFields[r + 1].interactable = true;
                }
                else
                {
                    // Finished S1 → jump to R6
                    resistanceFields[5].interactable = true;
                }
                break;

            case 3: // S2
                {
                    if (s2Row < s2Fields.Count - 1)
                    {
                        resistanceFields[currentRow + 1].interactable = true;
                    }
                    else
                    {
                        meanFields[0].interactable = true;
                    }
                    break;
                }


            case 4: // Mean
                if (r < meanFields.Count - 1)
                {
                    meanFields[r + 1].interactable = true;
                }
                else
                {
                    totalMeanField.interactable = true;
                }
                break;
        }
    }





}
