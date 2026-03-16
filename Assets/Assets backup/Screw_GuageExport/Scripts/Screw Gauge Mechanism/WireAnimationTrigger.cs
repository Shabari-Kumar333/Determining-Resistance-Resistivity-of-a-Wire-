using UnityEngine;

public class WireAnimationTrigger : MonoBehaviour
{
    [Header("Wire Animation")]
    public Animator wireAnimator;
    public string animationTriggerName = "PlaceWire";
    // Create this trigger parameter in Animator

    private bool hasPlayed = false;

    // Call this function when you want to place the wire
    public void TriggerWirePlacement()
    {
        if (!hasPlayed)
        {
            wireAnimator.SetTrigger(animationTriggerName);
            hasPlayed = true;
        }
    }

    // Optional: reset wire to starting position if needed
    public void ResetWire()
    {
        wireAnimator.Play("Idle"); // your idle animation/state
        hasPlayed = false;
    }
}
