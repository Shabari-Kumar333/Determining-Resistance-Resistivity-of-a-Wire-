using UnityEngine;
using System.Collections.Generic;
using TMPro;


[DefaultExecutionOrder(1000)]
public class MultiWireElectronFlow : MonoBehaviour
{
    [System.Serializable]
    public class WireFlow
    {
        public LineRenderer lineRenderer;
        public GameObject electronPrefab;
        public float speed = 0.3f;
        public float spacing = 0.15f;
        public int minCount = 5;
        public int maxCount = 100;

        [HideInInspector] public List<GameObject> electrons = new();
        [HideInInspector] public List<float> normalizedDistances = new();
        [HideInInspector] public List<Vector3> pathWorld = new();
        [HideInInspector] public List<float> segmentLengths = new();
        [HideInInspector] public float totalLength;
    }

    [Header("Wires")]
    public List<WireFlow> wires = new();

    [Header("Global Control")]
    public bool flowEnabled = true;
    [Header("External Control")]
    public TMP_Text controlText;


    // ================= UNITY =================

    void Start()
    {
        foreach (var wire in wires)
        {
            if (!wire.lineRenderer || !wire.electronPrefab)
                continue;

            BuildPool(wire);
            ResamplePath(wire);
            PlaceElectrons(wire);
        }
    }

    void LateUpdate()
    {
        foreach (var wire in wires)
        {
            if (!wire.lineRenderer)
                continue;

            ResamplePath(wire);
            bool electronsActive = ShouldElectronsBeActive();

            SetElectronVisibility(wire, electronsActive);


            if (electronsActive)
                UpdateFlow(wire);

        }
    }

    void SetElectronVisibility(WireFlow wire, bool visible)
    {
        for (int i = 0; i < wire.electrons.Count; i++)
        {
            if (wire.electrons[i].activeSelf != visible)
                wire.electrons[i].SetActive(visible);
        }
    }

    // ================= PATH =================

    void ResamplePath(WireFlow wire)
    {
        wire.pathWorld.Clear();
        wire.segmentLengths.Clear();
        wire.totalLength = 0f;

        int count = wire.lineRenderer.positionCount;
        if (count < 2)
            return;

        bool worldSpace = wire.lineRenderer.useWorldSpace;
        Transform t = wire.lineRenderer.transform;

        for (int i = 0; i < count; i++)
        {
            Vector3 p = wire.lineRenderer.GetPosition(i);
            if (!worldSpace)
                p = t.TransformPoint(p);

            wire.pathWorld.Add(p);
        }

        for (int i = 0; i < wire.pathWorld.Count - 1; i++)
        {
            float len = Vector3.Distance(
                wire.pathWorld[i],
                wire.pathWorld[i + 1]
            );

            if (len <= 0.0001f)
                continue;

            wire.segmentLengths.Add(len);
            wire.totalLength += len;
        }
    }

    // ================= ELECTRONS =================

    void BuildPool(WireFlow wire)
    {
        for (int i = wire.electrons.Count; i < wire.maxCount; i++)
        {
            GameObject e = Instantiate(wire.electronPrefab, transform);
            e.SetActive(false);
            wire.electrons.Add(e);
        }
    }

    void PlaceElectrons(WireFlow wire)
    {
        wire.normalizedDistances.Clear();

        if (wire.totalLength <= 0.001f)
            return;

        int count = Mathf.FloorToInt(wire.totalLength / wire.spacing);
        count = Mathf.Clamp(count, wire.minCount, wire.maxCount);

        float step = 1f / count;

        for (int i = 0; i < wire.electrons.Count; i++)
            wire.electrons[i].SetActive(i < count);

        for (int i = 0; i < count; i++)
        {
            float nd = step * i;
            wire.normalizedDistances.Add(nd);

            wire.electrons[i].transform.position =
                GetPositionAtNormalizedDistance(wire, nd);
        }
    }
    public void SetFlowState(bool state)
    {
        flowEnabled = state;

        bool electronsActive = ShouldElectronsBeActive();

        foreach (var wire in wires)
        {
            if (electronsActive)
            {
                ResamplePath(wire);
                PlaceElectrons(wire);
                SetElectronVisibility(wire, true);
            }
            else
            {
                SetElectronVisibility(wire, false);
            }
        }

        Debug.Log($"[ElectronFlow] Flow {(electronsActive ? "STARTED" : "STOPPED")}");
    }



    // ================= FLOW =================

    void UpdateFlow(WireFlow wire)
    {
        if (!flowEnabled || wire.totalLength <= 0.001f)
            return;

        float delta = (wire.speed / wire.totalLength) * Time.deltaTime;

        for (int i = 0; i < wire.normalizedDistances.Count; i++)
        {
            wire.normalizedDistances[i] += delta;

            // ONE-WAY FLOW (no loop)
            if (wire.normalizedDistances[i] >= 1f)
                wire.normalizedDistances[i] = 0f; // respawn at start

            wire.electrons[i].transform.position =
                GetPositionAtNormalizedDistance(
                    wire,
                    wire.normalizedDistances[i]
                );
        }
    }

    bool IsTextNonZero()
    {
        if (!controlText)
            return true;

        string text = controlText.text;

        // Remove unit (e.g. " V")
        text = text.Replace("V", "").Trim();

        if (float.TryParse(text, out float value))
            return !Mathf.Approximately(value, 0f);

        return true;
    }

    bool ShouldElectronsBeActive()
    {
        return flowEnabled && IsTextNonZero();
    }


    // ================= POSITION =================

    Vector3 GetPositionAtNormalizedDistance(WireFlow wire, float t)
    {
        float d = t * wire.totalLength;

        for (int i = 0; i < wire.segmentLengths.Count; i++)
        {
            float segLen = wire.segmentLengths[i];
            if (d <= segLen)
            {
                return Vector3.Lerp(
                    wire.pathWorld[i],
                    wire.pathWorld[i + 1],
                    d / segLen
                );
            }
            d -= segLen;
        }

        return wire.pathWorld[^1];
    }
}