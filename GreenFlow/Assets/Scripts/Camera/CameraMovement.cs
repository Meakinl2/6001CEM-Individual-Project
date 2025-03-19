using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private int moveSpeedMultiplier = 1, zoomStep = 5, minCameraSize = 10, maxCameraSize = 100;
    private Vector3 dragOrigin;

    private void Update()
    {
        PanCamera();
        ZoomCamera();
    }

    private void PanCamera() 
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 cursorDifference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += cursorDifference * moveSpeedMultiplier;   
        }
    }


    public void ZoomCamera() 
    {
        float newCameraSize = 0;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f )
        {
            newCameraSize = cam.orthographicSize + zoomStep;
            cam.orthographicSize = Mathf.Clamp(newCameraSize, minCameraSize, maxCameraSize);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f )
        {
            newCameraSize = cam.orthographicSize - zoomStep;
            cam.orthographicSize = Mathf.Clamp(newCameraSize, minCameraSize, maxCameraSize);
        }
  
    }

}
