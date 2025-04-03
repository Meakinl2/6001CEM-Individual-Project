using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class LaneRenderer : MonoBehaviour
{

    public List<Vector3> lanePoints;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.sortingOrder = 2;
        lineRenderer.enabled = false;

        lanePoints = new List<Vector3>();

    }

    public void UpdateLineRenderer() 
    {
        lineRenderer.positionCount = lanePoints.Count;
        lineRenderer.SetPositions(lanePoints.ToArray());
    }

}
