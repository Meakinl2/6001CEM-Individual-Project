using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance { get; private set; }
    // A dictionary that contains all Node objects by their GUID
    private Dictionary<string, Node> nodeRegistry;

    // A dictionary that shows all of the connections by Node id
    private Dictionary<string, HashSet<string>> nodeConnections;
    
    // A compare that allows for using Hashsets as dictionary keys such that the order of the contained elements won't matter.
    private HashSetComparer<string> hashsetComparer;
    
    // A dictionary that contains all the bezier control points, requires a hashset comparison object to allow the keys to be in any order.
    private Dictionary<HashSet<string>, BezierControl> connectionBeziers;

    public GameObject bezierControl;


    private void Awake()
    {
        // To prevent duplicate Object Managers
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        nodeRegistry = new Dictionary<string, Node>();
        hashsetComparer = new HashSetComparer<string>();
        nodeConnections = new Dictionary<string, HashSet<string>>();
        connectionBeziers = new Dictionary<HashSet<string>, BezierControl>(hashsetComparer);
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


    // Create a new connection between two nodes.
    public void AddNodeConnection(Node node1, Node node2)
    {
        HashSet<string> dictKey = new HashSet<string> {node1.id, node2.id};
        
        // Add Nodes to nodeConnections Dictionary if they aren't already there.
        if (!nodeConnections.ContainsKey(node1.id)) {nodeConnections.Add(node1.id, new HashSet<string>());}
        if (!nodeConnections.ContainsKey(node2.id)) {nodeConnections.Add(node2.id, new HashSet<string>());}
        
        // If Nodes aren't already listed as connections in nodeConnections dictionary to each other than add them.
        if (!nodeConnections[node1.id].Contains(node2.id)) {nodeConnections[node1.id].Add(node2.id);}
        if (!nodeConnections[node2.id].Contains(node1.id)) {nodeConnections[node2.id].Add(node1.id);}

        // If Nodes aren't already listed as connection in the Node objects then add them.
        if (!node1.connectedNodeIDs.Contains(node2.id)) {node1.connectedNodeIDs.Add(node2.id);}
        if (!node2.connectedNodeIDs.Contains(node1.id)) {node2.connectedNodeIDs.Add(node1.id);}

        if (connectionBeziers.ContainsKey(dictKey)) {return;}

        Vector2 bezierPosition = Vector2.Lerp(node1.transform.position, node2.transform.position, 0.5f);
        GameObject newBezierControl = Instantiate(bezierControl, bezierPosition, Quaternion.identity);
 
    }

    // Remove an exisiting connection between two nodes.
    public void RemoveNodeConnection(Node node1, Node node2)
    {
        HashSet<string> dictKey = new HashSet<string> {node1.id, node2.id};
       
        if (nodeConnections[node1.id].Contains(node2.id)) {nodeConnections[node1.id].Remove(node2.id);}
        if (nodeConnections[node2.id].Contains(node1.id)) {nodeConnections[node2.id].Remove(node1.id);}

        if (node1.connectedNodeIDs.Contains(node2.id)) {node1.connectedNodeIDs.Remove(node2.id);}
        if (node2.connectedNodeIDs.Contains(node1.id)) {node2.connectedNodeIDs.Remove(node1.id);}

    }

}