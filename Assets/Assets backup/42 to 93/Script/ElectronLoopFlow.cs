using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SkipInterval
{
    public Transform pointA;
    public Transform pointB;
}

public class ElectronLoopFlow : MonoBehaviour
{
    [Header("Setup")]
    public GameObject spherePrefab;

    [Header("Electron Paths")]
    public Transform[] path1;
    public Transform[] path2;
    public Transform[] path3;

    [Header("Flow Settings")]
    public float moveSpeed = 0.3f;
    public float sphereSpacing = 0.15f;
    public int maxPoolSize = 100;
    public int minPoolSize = 5;

    [Header("Skip Intervals")]
    public List<SkipInterval> skipIntervals = new();

    // ---------- INTERNAL ----------
    private FlowData flow1;
    private FlowData flowExtra;

    private bool usePath3 = false;
    private float swapTimer = -1f;

    void Awake()
    {
        flow1 = new FlowData(this);
        flowExtra = new FlowData(this);
    }

    void Start()
    {
        StartElectronFlow();
    }

    void Update()
    {
        flow1.Tick(moveSpeed);
        flowExtra.Tick(moveSpeed);

        // delayed swap logic (NO coroutine)
        if (swapTimer > 0f)
        {
            swapTimer -= Time.unscaledDeltaTime;
            if (swapTimer <= 0f)
            {
                ActivatePath3();
            }
        }
    }

    // ================= API =================

    public void StartElectronFlow()
    {
        flow1.Build(path1);
        flowExtra.Build(path2);
        usePath3 = false;
    }

    public void StopElectronFlow()
    {
        flow1.Disable();
        flowExtra.Disable();
    }

    public void SwapExtraPathWithDelay()
    {
        if (swapTimer > 0f) return; // prevent spam clicks
        swapTimer = 0.8f;          // path2 keeps flowing
    }


    private void ActivatePath3()
    {
        // stop & hide path2 FIRST (after delay)
        flowExtra.Disable();

        // start path3
        flowExtra.Build(path3);

        usePath3 = true;
        swapTimer = -1f;
    }


    // ================= FLOW DATA =================

    private class FlowData
    {
        private ElectronLoopFlow owner;
        private List<GameObject> spheres = new();
        private List<float> distances = new();
        private List<Vector3> path = new();
        private List<float> segments = new();
        private float totalLength;
        private bool active;

        public FlowData(ElectronLoopFlow owner)
        {
            this.owner = owner;
        }

        public void Build(Transform[] points)
        {
            Disable();

            path.Clear();
            segments.Clear();
            distances.Clear();
            totalLength = 0f;

            if (points == null || points.Length < 2) return;

            for (int i = 0; i < points.Length; i++)
                path.Add(points[i].position);
            path.Add(points[0].position);

            for (int i = 0; i < path.Count - 1; i++)
            {
                float len = Vector3.Distance(path[i], path[i + 1]);
                if (len > 0.0001f)
                {
                    segments.Add(len);
                    totalLength += len;
                }
            }

            if (totalLength <= 0.001f) return;

            for (int i = spheres.Count; i < owner.maxPoolSize; i++)
            {
                GameObject s = Instantiate(owner.spherePrefab, owner.transform);
                s.SetActive(false);
                spheres.Add(s);
            }

            int count = Mathf.Clamp(
                Mathf.FloorToInt(totalLength / owner.sphereSpacing),
                owner.minPoolSize,
                owner.maxPoolSize
            );

            float step = totalLength / count;

            for (int i = 0; i < spheres.Count; i++)
                spheres[i].SetActive(i < count);

            for (int i = 0; i < count; i++)
            {
                distances.Add(step * i);
                spheres[i].transform.position =
                    owner.GetPosition(path, segments, distances[i]);
            }

            active = true;
        }

        public void Tick(float speed)
        {
            if (!active || totalLength <= 0f) return;

            for (int i = 0; i < distances.Count; i++)
            {
                distances[i] += speed * Time.unscaledDeltaTime;
                if (distances[i] >= totalLength)
                    distances[i] -= totalLength;

                spheres[i].transform.position =
                    owner.GetPosition(path, segments, distances[i]);
            }
        }

        public void Disable()
        {
            active = false;
            foreach (var s in spheres)
                s.SetActive(false);
        }
    }

    // ================= POSITION =================

    private Vector3 GetPosition(
        List<Vector3> path,
        List<float> segments,
        float distance)
    {
        float d = distance;

        for (int i = 0; i < segments.Count; i++)
        {
            float len = segments[i];
            if (d <= len)
            {
                Vector3 a = path[i];
                Vector3 b = path[i + 1];

                foreach (var skip in skipIntervals)
                {
                    if (skip.pointA == null || skip.pointB == null) continue;
                    if ((a == skip.pointA.position && b == skip.pointB.position) ||
                        (a == skip.pointB.position && b == skip.pointA.position))
                        return b;
                }

                return Vector3.Lerp(a, b, d / len);
            }
            d -= len;
        }
        return path[0];
    }
}