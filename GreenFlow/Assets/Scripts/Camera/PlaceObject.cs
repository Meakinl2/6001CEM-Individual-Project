using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PlaceObject : MonoBehaviour
{
    public GameObject placeable;
    private Node currentSeletedNode; 

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
            
            if (currentSeletedNode != null) {
                newNode.AddConnectedNode(currentSeletedNode.id);
                currentSeletedNode.AddConnectedNode(newNode.id);
            }
                
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
                clickedNode.RemoveConnectedNode(currentSeletedNode.id);
                currentSeletedNode.RemoveConnectedNode(clickedNode.id);
                Color colour = new Color(255,255,255,1);
                clickedNode.UpdateColour(colour);
                UpdateSelectedNode(currentSeletedNode);
            } else {
                clickedNode.AddConnectedNode(currentSeletedNode.id);
                currentSeletedNode.AddConnectedNode(clickedNode.id);
                UpdateSelectedNode(currentSeletedNode);
            }
            
        }
    }

    private void CheckMiddleClick() {
        if (Input.GetMouseButtonDown(2))
        {
            GameObject selectedObject = GetTopObject();
            if (selectedObject == null || selectedObject.GetComponent<Node>() == null) {return;}

            
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
        Color colour;

        if (currentSeletedNode != null) 
        {
            colour = new Color(255,255,255,1);
            currentSeletedNode.UpdateColour(colour);
            
            foreach (string id in currentSeletedNode.connectedNodeIDs) 
            {
            Debug.Log("Ok");
            Node connectedNode = NodeManager.Instance.GetNodeByID(id);
            connectedNode.UpdateColour(colour);
            }
        }
        
        
        colour = new Color(0,0,0,1);
        node.UpdateColour(colour);

        colour = new Color(0,0,255,1);
        foreach (string id in node.connectedNodeIDs) 
        {
            Debug.Log("Ok");
            Node connectedNode = NodeManager.Instance.GetNodeByID(id);
            connectedNode.UpdateColour(colour);
        }

        currentSeletedNode = node;
    }
}

