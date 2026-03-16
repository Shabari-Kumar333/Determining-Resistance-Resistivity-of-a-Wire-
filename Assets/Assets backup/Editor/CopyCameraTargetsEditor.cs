using UnityEditor;
using UnityEngine;

public class CopyCameraTargetsEditor
{
    [MenuItem("Tools/Camera/Copy Camera Targets To StepPlayerAdapter")]
    static void CopyCameraTargets()
    {
        // Get selected GameObject
        GameObject go = Selection.activeGameObject;

        if (!go)
        {
            EditorUtility.DisplayDialog("Error", "Select a GameObject first", "OK");
            return;
        }

        // Source: CameraMovementController
        CameraMovementController source =
            go.GetComponentInChildren<CameraMovementController>();

        // Target: StepPlayerAdapter
        StepPlayerAdapter target =
            go.GetComponentInChildren<StepPlayerAdapter>();

        if (!source || !target)
        {
            EditorUtility.DisplayDialog(
                "Error",
                "CameraMovementController or StepPlayerAdapter not found in children",
                "OK"
            );
            return;
        }

        Undo.RecordObject(target, "Copy Camera Targets");

        target.cameraTargets = source.cameraTargets;

        EditorUtility.SetDirty(target);

        EditorUtility.DisplayDialog(
            "Success",
            $"Copied {source.cameraTargets.Length} camera targets to StepPlayerAdapter",
            "OK"
        );
    }
}
