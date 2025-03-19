using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance { get; private set; }
    private Dictionary<string, Node> nodeRegistry = new Dictionary<string, Node>();

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
        // Registers the new nodes so long as they don't already exist in the managers
        if (!nodeRegistry.ContainsKey(node.id))
        {
            nodeRegistry.Add(node.id, node);
            Debug.Log($"Object Registered: {node.id}");
        }
    }

    public Node GetNodeByID(string id)
    {
        return nodeRegistry.TryGetValue(id, out Node node) ? node : null;
    }
}