using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TableManager : MonoBehaviour
{
    // ===== Observation Table-2 (InputFields) =====
    public List<TMP_InputField> L;
    public List<TMP_InputField> S1;
    public List<TMP_InputField> Lp;
    public List<TMP_InputField> S2;
    public List<TMP_InputField> Smean;

    // ===== Observation Table-3 (Text) =====
    public List<TextMeshProUGUI> L_out;
    public List<TextMeshProUGUI> S1_out;
    public List<TextMeshProUGUI> Lp_out;
    public List<TextMeshProUGUI> S2_out;
    public List<TextMeshProUGUI> Smean_out;

    void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            L_out[i].text     = L[i].text;
            S1_out[i].text    = S1[i].text;
            Lp_out[i].text    = Lp[i].text;
            S2_out[i].text    = S2[i].text;
            Smean_out[i].text = Smean[i].text;
        }
    }
}
