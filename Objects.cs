using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Objects : MonoBehaviour
{
    //화면에서 물체를 클릭하면 움직이는 방법
    private Vector3 mOffset;
    private float mZCoord;
    GameObject Gizmos;

    private void Start()
    {
        Gizmos = GameObject.Find("translate_gizmo");
    }
    void OnMouseDown()
    {
        if (CameraControls.gizmoMoveOn)
        {
            mZCoord = Camera.main.WorldToScreenPoint(
              gameObject.transform.position).z;
            // Store offset = gameobject world pos - mouse world pos
            mOffset = gameObject.transform.position - GetMouseAsWorldPointZ();
        }
    }

    private Vector3 GetMouseAsWorldPointZ()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;
        // z coordinate of game object on screen
        mousePoint.z = mZCoord;
        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    void OnMouseDrag()
    {
        if (CameraControls.gizmoMoveOn)
        {
            transform.position = GetMouseAsWorldPointZ() + mOffset;
            Gizmos.transform.position = transform.position;
        }
    }
    
   
}
    