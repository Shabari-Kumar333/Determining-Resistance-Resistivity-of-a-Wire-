using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(LineRenderer))]
public class RopeOrWire : MonoBehaviour
{
    [Header("Rope Points")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Shape")]
    [Range(2, 100)]
    public int segments = 25;

    [Tooltip("Total rope length (greater than distance = sag)")]
    public float ropeLength = 10f;

    [Range(0.1f, 10f)]
    public float sagIntensity = 3f;

    [Header("Physics")]
    public float stiffness = 300f;
    public float damping = 15f;
    public Vector3 gravity = new Vector3(0, -9.81f, 0);
    public Vector3 wind;

    [Header("Visual")]
    public float ropeWidth = 0.1f;
    public bool taperEnds = true;

    private LineRenderer line;
    private Vector3 currentMidWorld;
    private Vector3 velocity;

    private void OnEnable()
    {
        SetupLineRenderer();
        ResetRope();
    }

    private void SetupLineRenderer()
    {
        if (!line)
            line = GetComponent<LineRenderer>();

        line.positionCount = segments + 1;

        // 🔴 IMPORTANT
        line.useWorldSpace = false;

        line.startWidth = ropeWidth;
        line.endWidth = ropeWidth;
        line.numCornerVertices = 4;
        line.numCapVertices = 4;

        if (taperEnds)
        {
            line.widthCurve = new AnimationCurve(
                new Keyframe(0, ropeWidth * 0.7f),
                new Keyframe(0.5f, ropeWidth),
                new Keyframe(1, ropeWidth * 0.7f)
            );
        }
    }

    private void ResetRope()
    {
        if (!startPoint || !endPoint)
            return;

        currentMidWorld = GetTargetMidpointWorld();
        velocity = Vector3.zero;
        UpdateLine();
    }

    private void FixedUpdate()
    {
        if (!startPoint || !endPoint)
            return;

        SimulatePhysics();
        UpdateLine();
    }

    // ================= PHYSICS (WORLD SPACE) =================

    private void SimulatePhysics()
    {
        Vector3 target = GetTargetMidpointWorld();

        Vector3 acceleration = (target - currentMidWorld) * stiffness;
        velocity += acceleration * Time.fixedDeltaTime;
        velocity *= Mathf.Clamp01(1f - damping * Time.fixedDeltaTime);

        velocity += (gravity + wind) * Time.fixedDeltaTime;
        currentMidWorld += velocity * Time.fixedDeltaTime;
    }

    private Vector3 GetTargetMidpointWorld()
    {
        Vector3 mid = Vector3.Lerp(startPoint.position, endPoint.position, 0.5f);

        float distance = Vector3.Distance(startPoint.position, endPoint.position);
        float sag = Mathf.Max(0, ropeLength - distance);

        mid.y -= sag * sagIntensity * 0.1f;
        return mid;
    }

    // ================= RENDERING (LOCAL SPACE) =================

    private void UpdateLine()
    {
        Vector3 startLocal = transform.InverseTransformPoint(startPoint.position);
        Vector3 endLocal = transform.InverseTransformPoint(endPoint.position);
        Vector3 midLocal = transform.InverseTransformPoint(currentMidWorld);

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;

            Vector3 point = QuadraticBezier(
                startLocal,
                midLocal,
                endLocal,
                t
            );

            line.SetPosition(i, point);
        }
    }

    private Vector3 QuadraticBezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        return (1 - t) * (1 - t) * a +
               2 * (1 - t) * t * b +
               t * t * c;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        SetupLineRenderer();
        ResetRope();
    }
#endif
}
