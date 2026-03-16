using UnityEngine;

namespace ResistivityExperiment
{
    public class ResistancePlug : MonoBehaviour
    {
        [Header("CONFIGURATION")]
        public float resistanceValue = 0f;

        [Header("Animation Settings")]
        public float liftHeight = 0.05f;
        public float moveSpeed = 2.0f;

        [Header("Status")]
        public bool isPluggedIn = true;

        private Vector3 initialPos;
        private Vector3 targetPos;
        private ResistanceBoxManager manager;

        void Start()
        {
            initialPos = transform.localPosition;
            targetPos = initialPos;

            manager = FindObjectOfType<ResistanceBoxManager>();
        }

        void Update()
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                targetPos,
                Time.deltaTime * moveSpeed
            );
        }

        void OnMouseDown()
        {
            TogglePlug();
        }

        void TogglePlug()
        {
            isPluggedIn = !isPluggedIn;

            if (isPluggedIn)
                targetPos = initialPos;
            else
                targetPos = initialPos + new Vector3(0, liftHeight, 0);

            if (manager != null)
                manager.RecalculateTotal();

            Debug.Log("Plug State: " + (isPluggedIn ? "Inserted" : "Removed"));
        }
    }
}