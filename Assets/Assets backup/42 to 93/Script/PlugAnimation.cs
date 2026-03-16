using UnityEngine;

public class PlugAnimation : MonoBehaviour
{
    private Animator animator;

    [Header("Objects to Activate")]
    public GameObject GameObjectToActivate;
    public GameObject GameObjectToActivate2;

    [Header("Electron Flows")]
    //public ElectronFlowOnWire electronFlowOnWire;
    public ElectronLoopFlow electronLoopFlow;

    private bool isAnimating = false;
    private bool isPluggedIn = false;

    [Range(0f, 1f)]
    public float plugInStopTime = 0.5f;

    void Awake()
    {
        animator = GetComponent<Animator>();

        // IMPORTANT: Do NOT Play here
        animator.speed = 1;
    }

    void Start()
    {
        // Freeze animation AFTER Animator initializes
        animator.speed = 0;
    }

    // 🔹 CALLED FROM TOUCH / RAYCAST
    public void PlayPlugAnimation()
    {
        if (isAnimating) return;

        Debug.Log("PlayPlugAnimation called");

        isAnimating = true;
        animator.speed = 1; // Resume animation
    }

    void Update()
    {
        if (!isAnimating) return;

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        // 🔹 Plug IN stop
        if (!isPluggedIn && info.normalizedTime >= plugInStopTime)
        {
            StopAnimation();
            isPluggedIn = true;
            OnPluggedIn();
        }
        // 🔹 Plug OUT stop
        else if (isPluggedIn && info.normalizedTime >= 1f)
        {
            StopAnimation();
            isPluggedIn = false;
            OnPluggedOut();

            // Reset for next cycle
            animator.Play(info.shortNameHash, 0, 1f);
            animator.speed = 0;
        }
    }

    void StopAnimation()
    {
        animator.speed = 0;
        isAnimating = false;
    }

    // 🔹 AFTER PLUG IN
    void OnPluggedIn()
    {
        if (GameObjectToActivate != null)
            GameObjectToActivate.SetActive(true);

        if (GameObjectToActivate2 != null)
            GameObjectToActivate2.SetActive(true);

        //if (electronFlowOnWire != null)
        //    electronFlowOnWire.ActivateElectronFlow();

        if (electronLoopFlow != null)
            electronLoopFlow.StartElectronFlow();
    }

    // 🔹 AFTER PLUG OUT
    void OnPluggedOut()
    {
        if (GameObjectToActivate != null)
            GameObjectToActivate.SetActive(false);

        if (GameObjectToActivate2 != null)
            GameObjectToActivate2.SetActive(false);

        //if (electronFlowOnWire != null)
        //    electronFlowOnWire.HideElectrons();

        if (electronLoopFlow != null)
            electronLoopFlow.StopElectronFlow();
    }
}
