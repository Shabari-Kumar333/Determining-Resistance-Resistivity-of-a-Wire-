using UnityEngine;

public class ShowRedLineAtBalanceLength : MonoBehaviour
{
    public S_Rightgap s_Rightgap;
    public Transform meterStart;
    public Transform meterEnd;
    public Transform redLine;

    public float meterLengthCm = 100f;
    public float fixedY = 0.04973028f;

    public void ShowRedLine()
    {
        if (!s_Rightgap || !meterStart || !meterEnd || !redLine) return;

        float t = s_Rightgap.balanceLength / meterLengthCm;

        // World position
        Vector3 worldPos = Vector3.Lerp(
            meterStart.position,
            meterEnd.position,
            t
        );

        worldPos.y = fixedY;

        // 🔥 FORCE WORLD SPACE
        redLine.SetParent(null, true);
        redLine.position = worldPos;

        redLine.gameObject.SetActive(true);
    }
}
