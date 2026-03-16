using UnityEngine;

public class Slide25Mechanism : MonoBehaviour
{
    void OnEnable()
    {
        var session = MeasurementSession.Instance;

        // 🔑 Switch system to Final Mean mode
        session.SetFinalMeanStage();

        // 🔍 If final mean is NOT yet locked, just debug expected value
        if (!session.HasFinalMean())
        {
            float expectedFinalMean = session.GetFinalMean();

            Debug.Log($"[FINAL MEAN EXPECTED] = {expectedFinalMean}");
            // ❗ DO NOT save here
            // Saving happens ONLY after correct user validation
        }
        else
        {
            // 🔒 Already validated earlier
            Debug.Log($"[FINAL MEAN LOCKED] = {session.finalMeanText}");
        }
    }
}
