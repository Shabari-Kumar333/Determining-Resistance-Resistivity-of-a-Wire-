using UnityEngine;

public class SpindleCollisionBlocker : MonoBehaviour
{
   // public ScrewGaugeMechanism screwGauge;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wire"))
        {
           // screwGauge.NotifyContact();
        }
    }
}
