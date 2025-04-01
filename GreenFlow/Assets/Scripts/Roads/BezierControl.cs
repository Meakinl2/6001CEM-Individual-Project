using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;

public class BezierControl : MonoBehaviour
{

    public string id { get; private set; }

    // Left and Right are completely aribitrary in this context, and serve only to distinguish
    private Node leftParent;
    private Node rightParent;

    private LineRenderer lineRenderer;
    
    // Unity burst isn't vector3 compatible and Native Arrays are faster than Lists.
    private NativeArray<float3> curvePoints;
    public int resolution = 20;
    private List<Vector3> curvePointsCache = new List<Vector3>();

    public bool isVisible = false;

    private void Awake()
    {
        id = Guid.NewGuid().ToString();
        NodeManager.Instance.RegisterBezierControl(this);

        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.sortingOrder = 1;
        lineRenderer.enabled = true;

        curvePoints = new NativeArray<float3>(resolution + 1, Allocator.Persistent);
    }

    private void OnDestroy()
    {
        // Ensure that the Native Array is emptied when the program closes or the object is destroyed to prevent memory leak
        if (curvePoints.IsCreated) curvePoints.Dispose();
    }

    // Set the Ids of the BezierControl parent nodes.
    public void SetParentNodes(Node parent1, Node parent2) 
    {
        leftParent = parent1;
        rightParent = parent2;
    }

    public void SetResolution(int newResolution) 
    {
        // Minimum evolution
        if (newResolution < 10) newResolution = 10;
        // Guard Clause for to skip unnecessary changes.
        if (newResolution == resolution) return;

        

        resolution = newResolution;

        // Dispose of old array and then create the new one
        if (curvePoints.IsCreated) 
            curvePoints.Dispose();

        curvePoints = new NativeArray<float3>(resolution + 1, Allocator.Persistent);
    }

    public void UpdateCurve()
    {
        // Guard clause to ensure that both parents are set
        if (leftParent == null || rightParent == null) return;

        // Approximation of bezier curve length so that the number of nodes can be dynamically chosen.

        float chordLength = Vector2.Distance(leftParent.transform.position, rightParent.transform.position);
        float controlOffsetLeft = Vector2.Distance(leftParent.transform.position, (2 * transform.position - leftParent.transform.position)) / 2;
        float controlOffsetRight = Vector2.Distance(rightParent.transform.position, (2 * transform.position - rightParent.transform.position)) / 2;

        float controlOffset = (controlOffsetLeft + controlOffsetRight) / 2;
        float weight = chordLength / (chordLength + Vector2.Distance(leftParent.transform.position, transform.position) 
                                                + Vector2.Distance(transform.position, rightParent.transform.position));

        float bezierLengthApprox = (chordLength + 2 * weight * controlOffset) / 2;
        

        SetResolution(Mathf.FloorToInt(bezierLengthApprox / 10));
        

        // Schedule parallel Bezier calculations
        var job = new BezierCurveJob
        {
            leftParentPos = math.float2(leftParent.transform.position.x, leftParent.transform.position.y),
            rightParentPos = math.float2(rightParent.transform.position.x, rightParent.transform.position.y),
            controlPos = math.float2(transform.position.x, transform.position.y),
            jobCurvePoints = curvePoints
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
        [NativeDisableParallelForRestriction] public NativeArray<float3> jobCurvePoints;

        public void Execute(int i) 
        {
            float t = i / (float)(jobCurvePoints.Length - 1);
            float2 p = math.pow(1 - t, 2) * leftParentPos + 2 * (1 - t) * t * controlPos + math.pow(t, 2) * rightParentPos;
            jobCurvePoints[i] = new float3(p.x, p.y, 0);
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
