using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

// using NodeManager;

public class Node : MonoBehaviour
{
    public string id {get; private set;}
    public List<string> connectedNodeIDs = new List<string>();

    private void Awake() 
    {
        id = Guid.NewGuid().ToString();
        NodeManager.Instance.RegisterNode(this);
    } 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void AddConnectedNode(string nodeID) 
    {
        if (connectedNodeIDs.Contains(nodeID)) {return;}
        Debug.Log($"Creating connection bewteen Node: {nodeID} and Node: {this.id}");
        connectedNodeIDs.Add(nodeID);
    }

    public void RemoveConnectedNode(string nodeID)
    {
        if (!connectedNodeIDs.Contains(nodeID)) {return;}
        Debug.Log($"Removing connection bewteen Node: {nodeID} and Node: {this.id}");
        connectedNodeIDs.Remove(nodeID);
    }

    public void UpdateColour(Color newColour) 
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {return;}

        spriteRenderer.color = newColour;
    }
}
