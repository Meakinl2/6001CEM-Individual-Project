using UnityEngine;
using System.Linq;

public class PlaceObject : MonoBehaviour
{

    public GameObject placeable;
    public Node[] connectedNodes;

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            // Get Mouse Positon and all the colliders at that point.
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition);
            
            if (hitColliders.Length > 0)
            {
                Collider2D topmostCollider = hitColliders
                    .OrderByDescending(c => c.GetComponent<SpriteRenderer>()?.sortingLayerID ?? 0) // Sorting Layer
                    .ThenByDescending(c => c.GetComponent<SpriteRenderer>()?.sortingOrder ?? 0) // Order in Layer
                    .First();

        GameObject topmostObject = topmostCollider.gameObject;
        Debug.Log("Topmost object: " + topmostObject.name);
            }
            else
            {
                Debug.Log("No objects at mouse position.");
            }
        }

        // For placing the desired object
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 offset = new Vector3(0,0,-9);
            GameObject newNode = Instantiate(placeable , pos+offset, Quaternion.identity);
            Node node = newNode.GetComponent<Node>();
            Debug.Log($"Instantiated Object with ID: {node.id}");
        }
    }
}

