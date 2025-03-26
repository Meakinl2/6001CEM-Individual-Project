using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PlaceObject : MonoBehaviour
{
    public GameObject placeable;
    private Node selectedNode; 
    private BezierControl selectedBezierControl;

    private bool isDraggingNode = false;
    private bool isDraggingBezierControl = false;

    private NodeManager nodeManager;
    
    private void Awake()
    {
        nodeManager = GetComponent<NodeManager>();
    }

    private void Update()
    {
        // Placing down new objects and selecting already placed objects
        CheckLeftClick();
        CheckDragging();

        // Creating and removing connections bewteen nodes, connections are one directional
        CheckRightClick();

        // Selecting objects so that nodes can be connected or deleted
        CheckMiddleClick();
        
    }


    private void CheckLeftClick() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject selectedObject = GetTopObject();

            if (selectedObject == null) {return;}

            if (selectedObject.GetComponent<Node>() != null) 
            {
                UpdateSelectedNode(selectedObject.GetComponent<Node>());
                Debug.Log($"Node Selected: {selectedNode.id}");
                isDraggingNode = true;
                return;
            }

            if (selectedObject.GetComponent<BezierControl>() != null)
            {
                selectedBezierControl = selectedObject.GetComponent<BezierControl>();
                Debug.Log($"BezierControl Selected: {selectedBezierControl.id}");
                isDraggingBezierControl = true;
                return;
            }

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject newObject = Instantiate(placeable , mousePosition, Quaternion.identity);
            Node newNode = newObject.GetComponent<Node>();
            
            if (selectedNode != null) {nodeManager.AddNodeConnection(selectedNode, newNode);}
                
            Debug.Log($"Instantiated Object with ID: {newNode.id}");
            UpdateSelectedNode(newNode);
        }
    }

    private void CheckDragging() 
    {
        if (!isDraggingNode && !isDraggingBezierControl) {return;}

        if (Input.GetMouseButtonUp(0)) {isDraggingNode = false; isDraggingBezierControl = false; return;}

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isDraggingNode) 
        {
            selectedNode.transform.position = mousePosition;
            foreach (string id in selectedNode.connectedNodeIDs) 
            {
                BezierControl bezierControl = nodeManager.GetBezierControlByParentIDs(selectedNode.id, id);
                bezierControl.UpdateSubNodes();
            }
        } 
        else if (isDraggingBezierControl) 
        {
            selectedBezierControl.transform.position = mousePosition;
            selectedBezierControl.UpdateSubNodes();
        }
    }


    private void CheckRightClick() 
    {
        // Guard Clause to make sure nothing is being dragged, else conflictions and errors may occur.
        if (isDraggingNode || isDraggingBezierControl) {return;}

        if (Input.GetMouseButtonDown(1))
        {
            
            GameObject selectedObject = GetTopObject();
            if (selectedObject == null || selectedObject.GetComponent<Node>() == null || selectedNode == null) {return;}

            Node clickedNode = selectedObject.GetComponent<Node>();

            if (clickedNode == selectedNode) {return;}

            if (selectedNode.connectedNodeIDs.Contains(clickedNode.id) || clickedNode.connectedNodeIDs.Contains(selectedNode.id)) 
            {
                nodeManager.RemoveNodeConnection(selectedNode, clickedNode);
                clickedNode.UpdateColourUnselected();
                UpdateSelectedNode(selectedNode);
            } else {
                nodeManager.AddNodeConnection(selectedNode, clickedNode);
                UpdateSelectedNode(selectedNode);
            }
            
        }
    }

    private void CheckMiddleClick() 
    {
        // Guard Clause to make sure nothing is being dragged, else conflictions and errors may occur.
        if (isDraggingNode || isDraggingBezierControl) {return;}

        if (Input.GetMouseButtonDown(2))
        {
            GameObject selectedObject = GetTopObject();
            if (selectedObject == null) {return;}

            if (selectedObject.GetComponent<Node>() != null) {

                Node clickedNode = selectedObject.GetComponent<Node>();

                if (clickedNode == selectedNode) {selectedNode = null;}
                nodeManager.DeregisterNode(clickedNode.id);
                Destroy(clickedNode.gameObject);
            }
            
        }
    }

    // Gets the topmost object at the mouse position
    private GameObject GetTopObject() 
    {
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

    private void UpdateSelectedNode(Node node) 
    {
        Debug.Log("Attempting to Update selectedNode to Node " + node.id);

        if (selectedNode != null) 
        {
            selectedNode.UpdateColourUnselected();
            
            foreach (string id in selectedNode.connectedNodeIDs) 
            {
                Node connectedNode = NodeManager.Instance.GetNodeByID(id);
                connectedNode.UpdateColourUnselected();

                BezierControl bezierControl = nodeManager.GetBezierControlByParentIDs(selectedNode.id, id);
                bezierControl.UpdateColourInvisible();
            }
        }

        node.UpdateColourSelected();

        foreach (string id in node.connectedNodeIDs) 
        {
            Node connectedNode = NodeManager.Instance.GetNodeByID(id);
            connectedNode.UpdateColourConnected();

            BezierControl bezierControl = nodeManager.GetBezierControlByParentIDs(node.id, id);
            bezierControl.UpdateColourUnselected();
        }

        selectedNode = node;

        Debug.Log("Successfully Updated selectedNode to Node " + node.id);
    }

    private void UpdateSelectedBezierControl(BezierControl bezierControl)
    {
        Debug.Log("Attempting to Update selectedBezierControl to BezierControl " + bezierControl.id);

        if (selectedBezierControl != null) 
        {
            selectedBezierControl.UpdateColourUnselected();
        }

        bezierControl.UpdateColourSelected();

        selectedBezierControl = bezierControl;
        Debug.Log("Successfully Updated selectedBezierControl to BezierControl " + bezierControl.id);
    }
}



