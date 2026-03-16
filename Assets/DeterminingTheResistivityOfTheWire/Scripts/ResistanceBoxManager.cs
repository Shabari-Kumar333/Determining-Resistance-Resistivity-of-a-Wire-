using UnityEngine;
using TMPro;

namespace ResistivityExperiment
{
    public class ResistanceBoxManager : MonoBehaviour
    {
        [Header("UI Text Object")]
        public TextMeshProUGUI valueText;

        [Header("Win Condition")]
        public float targetResistance = 15f;

        public GameObject correctObject;
        public GameObject wrongObject;

        private float currentTotal = 0f;

        public void RecalculateTotal()
        {
            float total = 0f;

            // Find all plugs
            ResistancePlug[] allPlugs = FindObjectsOfType<ResistancePlug>();

            foreach (ResistancePlug plug in allPlugs)
            {
                if (plug.isPluggedIn == false)
                {
                    total += plug.resistanceValue;
                }
            }

            currentTotal = total;
            UpdateUI();
        }

        void UpdateUI()
        {
            if (currentTotal == targetResistance)
            {
                if (valueText != null)
                {
                    valueText.text = "CORRECT! (" + currentTotal + " Ω)";
                    valueText.color = Color.green;
                }

                if (correctObject != null) correctObject.SetActive(true);
                if (wrongObject != null) wrongObject.SetActive(false);
            }
            else
            {
                if (valueText != null)
                {
                    valueText.text = "WRONG (" + currentTotal + " Ω)";
                    valueText.color = Color.red;
                }

                if (correctObject != null) correctObject.SetActive(false);
                if (wrongObject != null) wrongObject.SetActive(true);
            }
        }
    }
}