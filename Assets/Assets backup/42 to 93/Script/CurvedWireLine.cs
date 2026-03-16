using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
[RequireComponent(typeof(LineRenderer))]
public class AutoRoutedWire : MonoBehaviour
{
    [Header("Anchor Points (Only these!)")]
    public List<Transform> anchors = new List<Transform>();

    [Header("Wire Settings")]
    public float wireWidth = 0.05f;
    public int cornerSmoothness = 4;

    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
    }

    void Update()
    {
        if (anchors.Count < 2)
            return;

        GenerateWire();
    }

    void GenerateWire()
    {
        List<Vector3> wirePoints = new List<Vector3>();

        for (int i = 0; i < anchors.Count - 1; i++)
        {
            Vector3 start = anchors[i].position;
            Vector3 end = anchors[i + 1].position;

            wirePoints.Add(start);

            // Auto right-angle routing
            Vector3 corner = new Vector3(
                end.x,
                start.y,
                start.z
            );

            // Only add corner if it actually creates a bend
            if (corner != start && corner != end)
                wirePoints.Add(corner);
        }

        wirePoints.Add(anchors[anchors.Count - 1].position);

        line.positionCount = wirePoints.Count;
        line.SetPositions(wirePoints.ToArray());

        line.startWidth = wireWidth;
        line.endWidth = wireWidth;
        line.numCornerVertices = cornerSmoothness;
        line.numCapVertices = cornerSmoothness;
    }
}
