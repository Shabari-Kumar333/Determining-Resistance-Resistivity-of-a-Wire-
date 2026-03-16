using UnityEngine;

public class SharedRotateButtonHandler : MonoBehaviour
{
    private ISlideRotateHandler currentHandler;

    // Called by slides
    public void SetHandler(ISlideRotateHandler handler)
    {
        currentHandler = handler;
    }

    public void ClearHandler()
    {
        currentHandler = null;
    }

    // 🔘 ASSIGN THIS TO ROTATE BUTTON
    public void OnRotateButtonPressed()
    {
        if (currentHandler != null)
            currentHandler.HandleRotate();
    }
}
