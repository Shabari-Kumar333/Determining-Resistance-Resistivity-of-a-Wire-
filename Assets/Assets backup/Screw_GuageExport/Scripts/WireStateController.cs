using UnityEngine;
using System.Collections.Generic;

public class WireStateController : MonoBehaviour
{
    public int CurrentWireState { get; private set; } = 0;

    // slideIndex → wireStateIndex
    private Dictionary<int, int> slideStateMap = new Dictionary<int, int>();

    public void RotateToState(int slideIndex, int stateIndex)
    {
        slideStateMap[slideIndex] = stateIndex;
        CurrentWireState = stateIndex;

        Debug.Log($"[WIRE STATE] Active state = {stateIndex}");
    }

    public int GetStateForSlide(int slideIndex)
    {
        if (slideStateMap.TryGetValue(slideIndex, out int s))
            return s;

        return CurrentWireState;
    }
}
