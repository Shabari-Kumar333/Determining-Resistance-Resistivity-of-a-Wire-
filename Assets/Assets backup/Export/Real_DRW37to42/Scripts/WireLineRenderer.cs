using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class WireCheckpointList
{
    [Header("1. Path Points")]
    public List<Transform> checkpoints = new List<Transform>();

    [Header("2. Mesh To Copy (The Source)")]
    // Drag your Real 3D Wire here. 
    public GameObject meshToCopy;

    [Header("Events")]
    public UnityEvent onWireComplete;
}

public class WireLineRenderer : MonoBehaviour
{
    [Header("Animation Settings")]
    public float drawSpeed = 2f;
    public float wireWidth = 0.05f;

    [Header("Color Correction")]
    // If TRUE: It steals the color and makes it bright (Fixes dark lines).
    // If FALSE: It tries to copy the exact material (Might look dark).
    public bool fixDarkness = true;

    [Header("Wire Paths")]
    public List<WireCheckpointList> wireCheckpointLists = new List<WireCheckpointList>();

    private List<LineRenderer> lines = new List<LineRenderer>();

    void Awake()
    {
        CreateLineRenderers();
    }

    void Start()
    {
        StartDraw();
    }

    void CreateLineRenderers()
    {
        foreach (var old in lines) if (old != null) Destroy(old.gameObject);
        lines.Clear();

        for (int i = 0; i < wireCheckpointLists.Count; i++)
        {
            var segment = wireCheckpointLists[i];

            GameObject lineObj = new GameObject("WireSegment_" + i);
            lineObj.transform.SetParent(transform);

            LineRenderer lr = lineObj.AddComponent<LineRenderer>();

            lr.useWorldSpace = true;
            lr.startWidth = wireWidth;
            lr.endWidth = wireWidth;
            lr.numCornerVertices = 5;
            lr.numCapVertices = 5;
            lr.alignment = LineAlignment.View;

            // --- COLOR STEALING LOGIC ---
            if (segment.meshToCopy != null)
            {
                Renderer sourceRenderer = segment.meshToCopy.GetComponent<Renderer>();
                if (sourceRenderer != null)
                {
                    if (fixDarkness)
                    {
                        // 1. Find the Color (Works for Standard and URP)
                        Color stolenColor = Color.white;
                        Material mat = sourceRenderer.sharedMaterial;

                        if (mat.HasProperty("_BaseColor")) // URP Shader
                            stolenColor = mat.GetColor("_BaseColor");
                        else if (mat.HasProperty("_Color")) // Standard Shader
                            stolenColor = mat.color;

                        // 2. Apply to a Safe, Bright Shader
                        lr.material = new Material(Shader.Find("Sprites/Default"));
                        lr.startColor = stolenColor;
                        lr.endColor = stolenColor;
                    }
                    else
                    {
                        // Old method: Copy exact material (Risks looking dark)
                        lr.material = sourceRenderer.sharedMaterial;
                    }
                }

                // Hide the original mesh
                segment.meshToCopy.SetActive(false);
            }
            else
            {
                lr.material = new Material(Shader.Find("Sprites/Default"));
            }

            lr.positionCount = 0;
            lines.Add(lr);
        }
    }

    public void StartDraw()
    {
        StopAllCoroutines();
        StartCoroutine(DrawAllLists());
    }

    IEnumerator DrawAllLists()
    {
        for (int l = 0; l < wireCheckpointLists.Count; l++)
        {
            var checkpoints = wireCheckpointLists[l].checkpoints;
            var line = lines[l];
            var wireEvents = wireCheckpointLists[l];

            if (checkpoints == null || checkpoints.Count < 2) continue;

            line.enabled = true;
            yield return StartCoroutine(DrawSingleList(line, checkpoints));
            wireEvents.onWireComplete?.Invoke();
        }
    }

    IEnumerator DrawSingleList(LineRenderer line, List<Transform> checkpoints)
    {
        line.positionCount = 1;
        line.SetPosition(0, checkpoints[0].position);

        for (int i = 0; i < checkpoints.Count - 1; i++)
        {
            Vector3 start = checkpoints[i].position;
            Vector3 end = checkpoints[i + 1].position;

            float distance = Vector3.Distance(start, end);
            if (distance < 0.001f) distance = 0.001f;

            float t = 0f;
            line.positionCount++;

            while (t < 1f)
            {
                t += Time.deltaTime * drawSpeed / distance;
                line.SetPosition(line.positionCount - 1, Vector3.Lerp(start, end, t));
                yield return null;
            }

            line.SetPosition(line.positionCount - 1, end);
        }
    }
}