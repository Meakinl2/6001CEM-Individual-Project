using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance { get; private set; }
    // A collection off all current nodes and their id's
    private Dictionary<string, Node> nodeRegistry = new Dictionary<string, Node>();
    // A collection of all connected nodes
    


    private void Awake()
    {
        // To prevent duplicate Object Managers
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterNode(Node node)
    {
        if (!nodeRegistry.ContainsKey(node.id))
        {
            nodeRegistry.Add(node.id, node);
        }
    }

    public Node GetNodeByID(string id)
    {
        return nodeRegistry.TryGetValue(id, out Node node) ? node : null;
    } 

}