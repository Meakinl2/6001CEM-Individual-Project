using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance { get; private set; }
    // A dictionary that contains all Node objects by their GUID
    private Dictionary<string, Node> nodeRegistry;
    // A dictionary that contrains all BezierControls by GUID
    private Dictionary<string, BezierControl> bezierControlRegistry;

    // A dictionary that shows all of the connections by Node id
    private Dictionary<string, HashSet<string>> nodeConnections;
    
    // A compare that allows for using Hashsets as dictionary keys such that the order of the contained elements won't matter.
    private HashSetComparer<string> hashsetComparer;
    
    // A dictionary that contains all the bezier control points, requires a hashset comparison object to allow the keys to be in any order.
    private Dictionary<HashSet<string>, string> controlBezierConnections;
    
    public GameObject bezierControlObject;

    // Runs on creation to setup necessary factors
    private void Awake()
    {
        // To prevent duplicate Object Managers
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        nodeRegistry = new Dictionary<string, Node>();
        bezierControlRegistry = new Dictionary<string, BezierControl>();
        hashsetComparer = new HashSetComparer<string>();
        nodeConnections = new Dictionary<string, HashSet<string>>();
        controlBezierConnections = new Dictionary<HashSet<string>, string>(hashsetComparer);
    }

    // Adding a Node to the nodeRegistry
    public void RegisterNode(Node node)
    {
        Debug.Log("Attempting to Register Node: " + node.id);
        if (!nodeRegistry.ContainsKey(node.id)) nodeRegistry.Add(node.id, node);
        Debug.Log("Successfully Registered Node: " + node.id);
    }

    // Adding a BezierControl to the bezierControlRegsitry
    public void RegisterBezierControl(BezierControl bezierControl)
    {
        Debug.Log("Attempting to BezierControl: " + bezierControl.id);
        if (!bezierControlRegistry.ContainsKey(bezierControl.id)) {bezierControlRegistry.Add(bezierControl.id, bezierControl);}
        Debug.Log("Successfully to BezierControl: " + bezierControl.id);
    }

    // Remove Node from the nodeRegistry and also remove all connection entries
    public void DeregisterNode(string nodeId)
    {
        Debug.Log("Attempting to Deregister Node: " + nodeId);
        
        if (!nodeConnections.ContainsKey(nodeId)) return;

        // Copy required as the foreach loop edits the original hashset which causes the iteration to throw an error
        HashSet<string> connectionsHashSetCopy = new HashSet<string>(nodeConnections[nodeId]);
        Node node1 = GetNodeByID(nodeId);
        foreach (string node2Id in connectionsHashSetCopy) {
            Debug.Log("Trying to Remove Connection bewtween: " + nodeId + " And " + node2Id);
            Node node2 = GetNodeByID(node2Id);
            Debug.Log("Trying to Remove Connection bewtween2: " + node1 + " And " + node2);
            RemoveNodeConnection(node1, node2);
        }

        if (nodeRegistry.ContainsKey(nodeId)) nodeRegistry.Remove(nodeId);

        Debug.Log("Successfully Deregistered Node: " + nodeId);
    }

    // Remove BezierControl from the bezierControlRegistry
    public void DeregisterBezierControl(string bezierControlID)
    {
        Debug.Log("Attempting to Deregister BezierControl: " + bezierControlID);
        if (bezierControlRegistry.ContainsKey(bezierControlID)) {bezierControlRegistry.Remove(bezierControlID);}
        Debug.Log("Successfully Deregistered BezierControl: " + bezierControlID);
    }

    // Consult nodeRegsitry with Node ID and return Node if exists.
    public Node GetNodeByID(string id)
    {
        return nodeRegistry.TryGetValue(id, out Node node) ? node : null;
    } 

    // Consult bezierControlRegsitry with BezierControl ID and return BezierControl if exists.
    public BezierControl GetBezierControlByID(string id)
    {
        return bezierControlRegistry.TryGetValue(id, out BezierControl bezierControl) ? bezierControl : null;
    }

    // Gets all BezierControls that have the specified parent
    public List<BezierControl> GetBezierControlByParentID(string parentId)
    {
        List<string> bezierControlIds = controlBezierConnections.Where(kvp => kvp.Key.Contains(parentId))
                                                                .Select(kvp => kvp.Value)
                                                                .ToList();

        List<BezierControl> bezierControls = new List<BezierControl>() {};
        foreach (string id in bezierControlIds)
        {
            BezierControl nextBezierControl = GetBezierControlByID(id);
            bezierControls.Add(nextBezierControl);
        }
        return bezierControls;
    }


    // Consults controlBezierConnections with two NodeIds and returns ControlBezier object between them.
    public BezierControl GetBezierControlByParentIDs(string parent1Id, string parent2Id) 
    {
        HashSet<string> dictKey = new HashSet<string>() {parent1Id, parent2Id};
        string controlBezierId = controlBezierConnections.TryGetValue(dictKey, out string valueOutput) ? valueOutput : null;
        return GetBezierControlByID(controlBezierId);
    }

    // Create a new connection between two nodes, including intialising the BezierControl between them.
    public void AddNodeConnection(Node node1, Node node2)
    {
        Debug.Log("Attempting to add Connection between Node: " + node1.id + " and Node: " + node2.id);

        HashSet<string> dictKey = new HashSet<string> {node1.id, node2.id};
        
        // Add Nodes to nodeConnections Dictionary if they aren't already there.
        if (!nodeConnections.ContainsKey(node1.id)) nodeConnections.Add(node1.id, new HashSet<string>());
        if (!nodeConnections.ContainsKey(node2.id)) nodeConnections.Add(node2.id, new HashSet<string>());
        
        // If Nodes aren't already listed as connections in nodeConnections dictionary to each other than add them.
        if (!nodeConnections[node1.id].Contains(node2.id)) nodeConnections[node1.id].Add(node2.id);
        if (!nodeConnections[node2.id].Contains(node1.id)) nodeConnections[node2.id].Add(node1.id);

        // If Nodes aren't already listed as connection in the Node objects then add them.
        if (!node1.connectedNodeIDs.Contains(node2.id)) node1.connectedNodeIDs.Add(node2.id);
        if (!node2.connectedNodeIDs.Contains(node1.id)) node2.connectedNodeIDs.Add(node1.id);

        if (controlBezierConnections.ContainsKey(dictKey)) {return;}

        Vector2 bezierPosition = Vector2.Lerp(node1.transform.position, node2.transform.position, 0.5f);
        GameObject newBezierControlObject = Instantiate(bezierControlObject, bezierPosition, Quaternion.identity);
        BezierControl newBezierControl = newBezierControlObject.GetComponent<BezierControl>();
        controlBezierConnections.Add(dictKey, newBezierControl.id);
        newBezierControl.SetParentNodes(node1, node2);
        

        Debug.Log("Successfully added Connection between Node: " + node1.id + " and Node: " + node2.id);
 
    }

    // Remove an exisiting connection between two nodes, including deleting the BezierControl bewteen them
    public void RemoveNodeConnection(Node node1, Node node2)
    {
        Debug.Log("Attempting to remove Connection between Node: " + node1.id + " and Node: " + node2.id);

        HashSet<string> dictKey = new HashSet<string>() {node1.id, node2.id};
        
        if (nodeConnections[node1.id].Contains(node2.id)) nodeConnections[node1.id].Remove(node2.id);
        if (nodeConnections[node2.id].Contains(node1.id)) nodeConnections[node2.id].Remove(node1.id);
        
        if (node1.connectedNodeIDs.Contains(node2.id)) node1.connectedNodeIDs.Remove(node2.id);
        if (node2.connectedNodeIDs.Contains(node1.id)) node2.connectedNodeIDs.Remove(node1.id);
        
        if (!controlBezierConnections.ContainsKey(dictKey)) {return;}
        
        // Destroy the BezierControl that sits between the two nodes.
        BezierControl connectionBezier = GetBezierControlByID(controlBezierConnections[dictKey]);

        controlBezierConnections.Remove(dictKey);
        DeregisterBezierControl(connectionBezier.id);
        Destroy(connectionBezier.gameObject);
        
        Debug.Log("Successfully removed Connection between Node: " + node1.id + " and Node: " + node2.id);
    }


    public BezierControl GetRandomBezierControl()
    {
        string randomKey = bezierControlRegistry.Keys.ElementAt(Random.Range(0, bezierControlRegistry.Count));
        return bezierControlRegistry[randomKey];
    }

}