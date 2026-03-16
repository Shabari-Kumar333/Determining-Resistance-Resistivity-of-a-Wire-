using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace ResistivityExperiment
{
    public class SlideManager : MonoBehaviour
    {
        [Header("Slide Anchors (Camera Positions)")]
        public Transform[] slideAnchors;

        [Header("Slide Panels (UI)")]
        public GameObject[] slideUIs;

        [Header("Camera")]
        public Camera mainCam;
        public float camMoveSpeed = 2f;

        [Header("Navigation Buttons")]
        public Button btnNext;
        public Button btnPrev;

        [Header("Progress UI")]
        public TMP_Text slideNumberText;
        public Image[] progressDots;

        private int currentSlide = 0;

        void Start()
        {
            currentSlide = 0;

            if (btnNext != null)
                btnNext.onClick.AddListener(() => ChangeSlide(1));

            if (btnPrev != null)
                btnPrev.onClick.AddListener(() => ChangeSlide(-1));

            RefreshSlide();
        }

        public void ChangeSlide(int direction)
        {
            if (slideUIs == null || slideUIs.Length == 0)
                return;

            currentSlide += direction;
            currentSlide = Mathf.Clamp(currentSlide, 0, slideUIs.Length - 1);

            RefreshSlide();
        }

        void RefreshSlide()
        {
            UpdateSlideUI();
            UpdateProgressUI();
            MoveCamera();
        }

        void UpdateSlideUI()
        {
            if (slideUIs == null) return;

            for (int i = 0; i < slideUIs.Length; i++)
            {
                if (slideUIs[i] != null)
                    slideUIs[i].SetActive(i == currentSlide);
            }
        }

        void UpdateProgressUI()
        {
            if (slideNumberText != null && slideUIs != null)
                slideNumberText.text = (currentSlide + 1) + "/" + slideUIs.Length;

            if (progressDots != null)
            {
                for (int i = 0; i < progressDots.Length; i++)
                {
                    if (progressDots[i] != null)
                        progressDots[i].enabled = (i == currentSlide);
                }
            }
        }

        void MoveCamera()
        {
            if (slideAnchors == null || slideAnchors.Length == 0) return;
            if (currentSlide >= slideAnchors.Length) return;
            if (slideAnchors[currentSlide] == null) return;
            if (mainCam == null) return;

            StopAllCoroutines();
            StartCoroutine(SmoothCamMove(slideAnchors[currentSlide]));
        }

        IEnumerator SmoothCamMove(Transform target)
        {
            Vector3 startPos = mainCam.transform.position;
            Quaternion startRot = mainCam.transform.rotation;

            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime * camMoveSpeed;

                mainCam.transform.position = Vector3.Lerp(startPos, target.position, t);
                mainCam.transform.rotation = Quaternion.Slerp(startRot, target.rotation, t);

                yield return null;
            }
        }

        public void ChangeSlideExternally(int dir)
        {
            ChangeSlide(dir);
        }
    }
}