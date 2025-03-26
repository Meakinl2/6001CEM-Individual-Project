using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

// using NodeManager;

public class Node : MonoBehaviour
{
    public string id { get; private set; }
    public HashSet<string> connectedNodeIDs = new HashSet<string>();

    private void Awake()
    {
        id = Guid.NewGuid().ToString();
        NodeManager.Instance.RegisterNode(this);
    }

    // Method for adding a node connection.
    public void AddConnectedNode(string nodeID)
    {
        if (connectedNodeIDs.Contains(nodeID)) { return; }
        Debug.Log($"Creating connection bewteen Node: {nodeID} and Node: {this.id}");
        connectedNodeIDs.Add(nodeID);
    }

    // Method for removing a node connection.
    public void RemoveConnectedNode(string nodeID)
    {
        if (!connectedNodeIDs.Contains(nodeID)) { return; }
        Debug.Log($"Removing connection bewteen Node: {nodeID} and Node: {this.id}");
        connectedNodeIDs.Remove(nodeID);
    }


    public void UpdateColour(Color newColour)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) { return; }

        spriteRenderer.color = newColour;
    }


    public void UpdateColourUnselected() 
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) { return; }

        spriteRenderer.color = new Color(255,255,255,1);
    }


    public void UpdateColourSelected() 
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) { return; }

        spriteRenderer.color = new Color(0,0,0,1);
    }

    public void UpdateColourConnected() 
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) { return; }

        spriteRenderer.color = new Color(0,0,255,1);
    }

    public void UpdateColourInvisible()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) { return; }

        spriteRenderer.color = new Color(0,0,0,0);

    }
}
