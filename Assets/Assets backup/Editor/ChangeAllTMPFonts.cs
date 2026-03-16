using UnityEditor;
using UnityEngine;
using TMPro;

public class ChangeAllTMPFonts : EditorWindow
{
    TMP_FontAsset newFont;

    [MenuItem("Tools/Change All TMP Fonts")]
    public static void ShowWindow()
    {
        GetWindow<ChangeAllTMPFonts>("Change TMP Fonts");
    }

    void OnGUI()
    {
        GUILayout.Label("Replace ALL TextMeshPro Fonts", EditorStyles.boldLabel);

        newFont = (TMP_FontAsset)EditorGUILayout.ObjectField(
            "New Font",
            newFont,
            typeof(TMP_FontAsset),
            false
        );

        if (GUILayout.Button("Replace Fonts"))
        {
            if (newFont == null)
            {
                Debug.LogError("Please assign a TMP Font Asset!");
                return;
            }

            ReplaceFonts();
        }
    }

    void ReplaceFonts()
    {
        TMP_Text[] texts = Resources.FindObjectsOfTypeAll<TMP_Text>();

        int count = 0;
        foreach (TMP_Text text in texts)
        {
            Undo.RecordObject(text, "Change TMP Font");
            text.font = newFont;
            EditorUtility.SetDirty(text);
            count++;
        }

        Debug.Log($"Replaced fonts on {count} TextMeshPro objects.");
    }
}
