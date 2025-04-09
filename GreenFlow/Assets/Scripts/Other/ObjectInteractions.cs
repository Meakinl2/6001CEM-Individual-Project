using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ObjectInteractions : MonoBehaviour
{
    public GameObject placeable;
    private Node selectedNode; 
    private BezierControl selectedBezierControl;

    private bool isDraggingNode = false;
    private bool isDraggingBezierControl = false;

    private NodeManager nodeManager;
    private VehicleManager vehicleManager;
    
    private void Awake()
    {
        nodeManager = GetComponent<NodeManager>();
        vehicleManager = GetComponent<VehicleManager>();
    }


    private void Update()
    {
        
        // Check that the UI isn't being interated with.
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        // Check to see if the vehicle spawner is active or not.
        CheckVPress();
        if (vehicleManager.isActive) return;

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
            // Select the topmost object
            GameObject selectedObject = GetTopObject();

            // Guard clause for if there is no object to select
            if (selectedObject == null) {return;}

            // Check if the object is a node
            if (selectedObject.GetComponent<Node>() != null) 
            {
                // Update Selected Node
                UpdateSelectedNode(selectedObject.GetComponent<Node>());
                Debug.Log($"Node Selected: {selectedNode.id}");
                // Start Dragging the Node
                isDraggingNode = true;
                return;
            }

            // Check if the object is a BezierCOntrol
            if (selectedObject.GetComponent<BezierControl>() != null)
            {
                // Select the Bezier Control
                selectedBezierControl = selectedObject.GetComponent<BezierControl>();
                Debug.Log($"BezierControl Selected: {selectedBezierControl.id}");
                // Start Dragging the Bezier Control
                isDraggingBezierControl = true;
                return;
            }

            // If neither of the previous checks were true then instantiate a new
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject newObject = Instantiate(placeable , mousePosition, Quaternion.identity);
            Node newNode = newObject.GetComponent<Node>();
            
            // Assuming there is one connect the new node to the currently selected Node
            if (selectedNode != null) {nodeManager.AddNodeConnection(selectedNode, newNode);}
                
            Debug.Log($"Instantiated Object with ID: {newNode.id}");
            // Make the new node the currently selected one.
            UpdateSelectedNode(newNode);
        }
    }

    private void CheckDragging() 
    {
        // Check something actually is being dragged
        if (!isDraggingNode && !isDraggingBezierControl) return;

        // If button has been released then stop dragging 
        if (Input.GetMouseButtonUp(0) && isDraggingNode) 
        {
            isDraggingNode = false; 

            // Then update the rest of the connected bezier control
            foreach (string id in selectedNode.connectedNodeIDs) 
            {
                BezierControl bezierControl = nodeManager.GetBezierControlByParentIDs(selectedNode.id, id);
                bezierControl.UpdateLanePoints();
            }

            return;
        }
        // Same check but for Bezier Control
        else if (Input.GetMouseButtonUp(0) && isDraggingBezierControl)
        {
            isDraggingBezierControl = false; 
            selectedBezierControl.UpdateLanePoints();
            return;
        }


        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isDraggingNode) 
        {
            selectedNode.transform.position = mousePosition;
            foreach (string id in selectedNode.connectedNodeIDs) 
            {
                BezierControl bezierControl = nodeManager.GetBezierControlByParentIDs(selectedNode.id, id);
                bezierControl.UpdateCurve();
            }
        } 
        else if (isDraggingBezierControl) 
        {
            selectedBezierControl.transform.position = mousePosition;
            selectedBezierControl.UpdateCurve();
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


    private void CheckVPress()
    {
        if (Input.GetKeyDown(KeyCode.V)) 
        {
            if (nodeManager.bezierControlRegistry.Count < 1) return;
            if (!vehicleManager.isActive) 
            {
                vehicleManager.StartSpawning();
                return;
            }
            Debug.Log("Test");
            vehicleManager.StopSpawning();
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



