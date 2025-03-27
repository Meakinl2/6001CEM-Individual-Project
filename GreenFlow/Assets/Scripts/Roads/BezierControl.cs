using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class BezierControl : MonoBehaviour
{

    public string id { get; private set; }

    // Left and Right are completely aribitrary in this context, and serve only to distinguish
    private Node leftParent;
    private Node rightParent;

    public GameObject subNodeObject;
    public List<GameObject> subNodes = new List<GameObject>();

    private LineRenderer lineRenderer;
    private List<Vector3> lineRendererPoints = new List<Vector3>();

    public bool isVisible = false;

    private void Awake()
    {
        id = Guid.NewGuid().ToString();
        NodeManager.Instance.RegisterBezierControl(this);

        lineRenderer = gameObject.GetComponent<LineRenderer>();
        
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;
        lineRenderer.sortingOrder = 1;
        lineRenderer.enabled = true;
    }

    // Set the Ids of the BezierControl parent nodes.
    public void SetParentNodes(Node parent1, Node parent2) 
    {
        leftParent = parent1;
        rightParent = parent2;
    }


    public void UpdateSubNodes() 
    {

        DestroySubnodes();

        for (float t = 0; t <= 1; t += 0.05f) 
        {
            Vector2 pos = Mathf.Pow(1 - t, 2) * leftParent.transform.position
                        + 2 * (1 - t) * t * transform.position
                        + Mathf.Pow(t, 2) * rightParent.transform.position;

            GameObject subNode = Instantiate(subNodeObject, pos, Quaternion.identity);
            subNodes.Add(subNode);
            lineRendererPoints.Add(subNode.transform.position);
        }   

        lineRendererPoints.Add(rightParent.transform.position);
        lineRenderer.positionCount = lineRendererPoints.Count;
        lineRenderer.SetPositions(lineRendererPoints.ToArray());

    }

    public void DestroySubnodes() 
    {
        foreach (GameObject subNode in subNodes) {  Destroy(subNode); }
        lineRendererPoints.Clear();
    }


    public void UpdateColourUnselected() 
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) { return; }

        spriteRenderer.color = new Color32(200,200,200,120);
    }

    public void UpdateColourSelected() 
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) { return; }

        spriteRenderer.color = new Color32(100,100,100,120);
    }

    public void UpdateColourInvisible()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) { return; }

        spriteRenderer.color = new Color32(0,0,0,0);

    }
}
