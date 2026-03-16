using UnityEngine;

public class SlideSetController : MonoBehaviour
{
    public GameObject set1;
    public GameObject set2;
    public GameObject set3;

    public SlideManager set1Manager;
    public SlideManager_TH set2Manager;
    public StepPlayerAdapter set3StepAdapter;

    public void ActivateSet(int set)
    {
        set1.SetActive(set == 1);
        set2.SetActive(set == 2);
        set3.SetActive(set == 3);
    }

    public void SetLocalSlide(int set, int index)
    {
        if (set == 1) set1Manager.SetSlide(index);
        else if (set == 2) set2Manager.SetSlide(index);
        else if (set == 3) set3StepAdapter.SetStep(index);
    }
}
