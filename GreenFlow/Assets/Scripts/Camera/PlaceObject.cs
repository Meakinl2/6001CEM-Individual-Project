using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PlaceObject : MonoBehaviour
{
    public GameObject placeable;
    private Node currentSeletedNode; 

    private NodeManager nodeManager;
    
    private void Awake()
    {
        nodeManager = GetComponent<NodeManager>();
    }

    private void Update()
    {
        // Placing down new objects and selecting already placed objects
        CheckLeftClick();

        // Creating and removing connections bewteen nodes, connections are one directional
        CheckRightClick();

        // Selecting objects so that nodes can be connected or deleted
        CheckMiddleClick();
        
    }

    private void CheckLeftClick() {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject selectedObject = GetTopObject();

            if (selectedObject == null) {return;}

            if (selectedObject.GetComponent<Node>() != null) 
            {
                UpdateSelectedNode(selectedObject.GetComponent<Node>());
                Debug.Log($"Node Selected: {currentSeletedNode.id}");
                return;
            }

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject newObject = Instantiate(placeable , mousePosition, Quaternion.identity);
            Node newNode = newObject.GetComponent<Node>();
            
            if (currentSeletedNode != null) {nodeManager.AddNodeConnection(currentSeletedNode, newNode);}
                
            Debug.Log($"Instantiated Object with ID: {newNode.id}");
            UpdateSelectedNode(newNode);
        }
    }

    private void CheckRightClick() {
        if (Input.GetMouseButtonDown(1))
        {
            GameObject selectedObject = GetTopObject();
            if (selectedObject == null || selectedObject.GetComponent<Node>() == null || currentSeletedNode == null) {return;}

            Node clickedNode = selectedObject.GetComponent<Node>();

            if (clickedNode == currentSeletedNode) {return;}

            if (currentSeletedNode.connectedNodeIDs.Contains(clickedNode.id) || clickedNode.connectedNodeIDs.Contains(currentSeletedNode.id)) 
            {
                nodeManager.RemoveNodeConnection(currentSeletedNode, clickedNode);
                clickedNode.UpdateColour(new Color(255,255,255,1));
                UpdateSelectedNode(currentSeletedNode);
            } else {
                nodeManager.AddNodeConnection(currentSeletedNode, clickedNode);
                UpdateSelectedNode(currentSeletedNode);
            }
            
        }
    }

    private void CheckMiddleClick() {
        if (Input.GetMouseButtonDown(2))
        {
            GameObject selectedObject = GetTopObject();
            if (selectedObject == null) {return;}

            if (selectedObject.GetComponent<Node>() != null) {

                Node clickedNode = selectedObject.GetComponent<Node>();

                if (clickedNode == currentSeletedNode) {currentSeletedNode = null;}
                nodeManager.DeregisterNode(clickedNode.id);
                Destroy(clickedNode.gameObject);
            }
            
        }
    }

    // Gets the topmost object at the mouse position
    private GameObject GetTopObject() {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition);
        
        if (hitColliders.Length <= 0) {Debug.Log("No objects at mouse position."); return null;}
        Collider2D topCollider = hitColliders
            .OrderByDescending(c => c.GetComponent<SpriteRenderer>()?.sortingLayerID ?? 0) // Sorting Layer
            .ThenByDescending(c => c.GetComponent<SpriteRenderer>()?.sortingOrder ?? 0) // Order in Layer
            .First();

        GameObject selectedObject = topCollider.gameObject;
        return selectedObject;
    }

    private void UpdateSelectedNode(Node node) {
        Debug.Log("Attempting to Update SelectedNode to Node " + node.id);

        if (currentSeletedNode != null) 
        {
            currentSeletedNode.UpdateColour(new Color(255,255,255,1));
            
            foreach (string id in currentSeletedNode.connectedNodeIDs) 
            {
            Node connectedNode = NodeManager.Instance.GetNodeByID(id);
            connectedNode.UpdateColour(new Color(255,255,255,1));
            }
        }

        node.UpdateColour(new Color(0,0,0,1));

        foreach (string id in node.connectedNodeIDs) 
        {
            Debug.Log("UpdateSelectedNodeNode Connected Node: " + id);
            Node connectedNode = NodeManager.Instance.GetNodeByID(id);
            connectedNode.UpdateColour(new Color(0,0,255,1));
        }

        currentSeletedNode = node;

        Debug.Log("Successfully Updated SelectedNode to Node " + node.id);
    }
}

