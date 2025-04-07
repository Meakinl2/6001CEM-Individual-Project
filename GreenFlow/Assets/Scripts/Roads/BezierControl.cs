using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;

// using LineManager;
public class BezierControl : MonoBehaviour
{

    public string id { get; private set; }

    // Left and Right are completely aribitrary in this context, and serve only to distinguish
    private Node leftParent;
    private Node rightParent;

    public bool isVisible = false;

    private int numLanes = 1;
    private float laneWidth = 3.5F;

    
    // Unity burst isn't vector3 compatible and Native Arrays are faster than Lists.
    private int resolution = 20;
    private NativeArray<float3> curvePoints;
    private NativeArray<float3> normalVectors;
    private List<Vector3> curvePointsCache = new List<Vector3>();
    
    public GameObject laneObject;
    private LineRenderer lineRenderer;

    
    private void Awake()
    {
        id = Guid.NewGuid().ToString();
        NodeManager.Instance.RegisterBezierControl(this);

        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.sortingOrder = 1;
        lineRenderer.enabled = true;

        curvePoints = new NativeArray<float3>(resolution + 1, Allocator.Persistent);
        normalVectors = new NativeArray<float3>(resolution + 1, Allocator.Persistent);

        UpdateRoadWidth();
    }

    private void OnDestroy()
    {
        // Ensure that the Native Array is emptied when the program closes or the object is destroyed to prevent memory leak
        if (curvePoints.IsCreated) curvePoints.Dispose();
        if (normalVectors.IsCreated) normalVectors.Dispose();

    }

    // Set the Ids of the BezierControl parent nodes.
    public void SetParentNodes(Node parent1, Node parent2) 
    {
        leftParent = parent1;
        rightParent = parent2;
        UpdateCurve();
        UpdateLanePoints();
        AddArbitraryLaneConnections();
    }

    public void SetResolution(int newResolution) 
    {
        // Minimum evolution
        if (newResolution < 10) newResolution = 10;
        // Guard Clause for to skip unnecessary changes.
        if (newResolution == resolution) return;

        resolution = newResolution;

        // Dispose of old array and then create the new one
        if (curvePoints.IsCreated) curvePoints.Dispose();
        if (normalVectors.IsCreated) normalVectors.Dispose();

        curvePoints = new NativeArray<float3>(resolution + 1, Allocator.Persistent);
        normalVectors = new NativeArray<float3>(resolution + 1, Allocator.Persistent);
    }


    private int CalculateResolution() {
        float chordLength = Vector2.Distance(leftParent.transform.position, rightParent.transform.position);
        float controlOffsetLeft = Vector2.Distance(leftParent.transform.position, (2 * transform.position - leftParent.transform.position)) / 2;
        float controlOffsetRight = Vector2.Distance(rightParent.transform.position, (2 * transform.position - rightParent.transform.position)) / 2;

        float controlOffset = (controlOffsetLeft + controlOffsetRight) / 2;
        float weight = chordLength / (chordLength + Vector2.Distance(leftParent.transform.position, transform.position) 
                                                + Vector2.Distance(transform.position, rightParent.transform.position));

        float bezierLengthApprox = (chordLength + 2 * weight * controlOffset) / 2;

        return Mathf.FloorToInt(bezierLengthApprox / 10);
    }


    public void UpdateRoadWidth() 
    {
        float width = (float)laneWidth * (float)numLanes * (float)2;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }


    public void UpdateCurve()
    {
        // Guard clause to ensure that both parents are set
        if (leftParent == null || rightParent == null) return;

        // Approximation of bezier curve length so that the number of nodes can be dynamically chosen.
        int newResolution = CalculateResolution();
        SetResolution(newResolution);
    
        // Schedule parallel Bezier calculations
        var job = new BezierCurveJob
        {
            leftParentPos = math.float2(leftParent.transform.position.x, leftParent.transform.position.y),
            rightParentPos = math.float2(rightParent.transform.position.x, rightParent.transform.position.y),
            controlPos = math.float2(transform.position.x, transform.position.y),
            curvePoints = curvePoints,
            normalVectors = normalVectors
        };

        JobHandle handle = job.Schedule(curvePoints.Length, 3);
        handle.Complete();

        // Update LineRenderer with computed points
        Vector3[] positions = new Vector3[curvePoints.Length];
        for (int i = 0; i < curvePoints.Length; i++) 
            positions[i] = curvePoints[i];

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }


    [BurstCompile]
    private struct BezierCurveJob: IJobParallelFor
    {
        public float2 leftParentPos, rightParentPos, controlPos;
        // Allow the native array to be accessed by the parallel function
        [NativeDisableParallelForRestriction] public NativeArray<float3> curvePoints;
        [NativeDisableParallelForRestriction] public NativeArray<float3> normalVectors;

        public void Execute(int i) 
        {
            float t = i / (float)(curvePoints.Length - 1);

            float2 point = math.pow(1 - t, 2) * leftParentPos + 2 * (1 - t) * t * controlPos + math.pow(t, 2) * rightParentPos;
        
            curvePoints[i] = new float3(point.x, point.y, 0);

            float2 tangent = -2 * (1 - t) * leftParentPos + 2 * (1 - 2 * t) * controlPos + 2 * t * rightParentPos;
            tangent = math.normalize(tangent);

            normalVectors[i] = new float3(-tangent.y, tangent.x, 0);
        }
    }


    // Kept Seperate so that it can be called when a curve stops being altered rather than every frame like UpdateCurve is
    public void UpdateLanePoints() 
    {
        foreach (int polarity in new int[] {-1, 1}) 
        {
            for (int i = 1; i <= numLanes; i++) 
            {
                int laneIndex = i * polarity;
                
                List<Vector3> lanePoints =  new List<Vector3>();
                for (int j = 0; j <= resolution; j++)
                {
                    lanePoints.Add(curvePoints[j] + normalVectors[j] * (laneWidth / 2) * laneIndex);
                }

                if (polarity == -1) lanePoints.Reverse();

                LaneManager.Instance.TryRegisterLane(id, laneIndex);
                LaneManager.Instance.UpdateLaneBlob(id, laneIndex, lanePoints);
            }
        }
    }

    // Arbitrarily connects all lanes 
    public void AddArbitraryLaneConnections() 
    {
        List<BezierControl> leftParentBeziers = NodeManager.Instance.GetBezierControlByParentID(leftParent.id);
        List<BezierControl> rightParentBeziers = NodeManager.Instance.GetBezierControlByParentID(rightParent.id);

        leftParentBeziers.Remove(this);
        rightParentBeziers.Remove(this);

        foreach (BezierControl leftbezierConnections in leftParentBeziers)
        {
            int polarity = 1;
            
            if (leftbezierConnections.leftParent == leftParent) polarity = -1;
                
            for (int i = 1; i <= numLanes; i ++) 
            {
                for (int j = 1; j <= leftbezierConnections.numLanes; j++)
                {
                    LaneManager.Instance.AddLaneConnection(id, i, leftbezierConnections.id, j * polarity);
                }
            }    
        }

        foreach (BezierControl rightbezierConnections in leftParentBeziers)
        {
            int polarity = -1;
            
            if (rightbezierConnections.rightParent == rightParent) polarity = 1;
                
            for (int i = 1; i <= numLanes; i ++) 
            {
                for (int j = 1; j <= rightbezierConnections.numLanes; j++)
                {
                    LaneManager.Instance.AddLaneConnection(id, -i, rightbezierConnections.id, j * polarity);
                }
            }    
        }

        for (int i = 1; i <= numLanes; i ++) 
        {
            for (int j = 1; j <= numLanes; j ++)
            {
                LaneManager.Instance.AddLaneConnection(id, i, id, -j);
                LaneManager.Instance.AddLaneConnection(id, -i, id, j);
            }
            
        }
    }

    // Colour for if BezierControl is visible but not selected
    public void UpdateColourUnselected() 
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) { return; }

        spriteRenderer.color = new Color32(200,200,200,120);
    }

    // Colour for if BezierControl is visible and selected
    public void UpdateColourSelected() 
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) { return; }

        spriteRenderer.color = new Color32(100,100,100,120);
    }

    // Colour for making BezierControl invisible
    public void UpdateColourInvisible()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) { return; }

        spriteRenderer.color = new Color32(0,0,0,0);

    }
}
