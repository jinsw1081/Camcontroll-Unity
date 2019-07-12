using UnityEngine;

public class GzimoMove : MonoBehaviour
{
    public int HowToMove;
    private Vector3 mOffset;
    private float mZCoord;
    
    
    void OnMouseDrag()
    {
        float distance = Vector3.Distance(transform.root.position, Camera.main.transform.position);
        switch (HowToMove)
        {
            case 0:
                float deltaX = Input.GetAxis("Mouse X") * (Time.deltaTime * 10 * distance);
                mOffset = Camera.main.transform.right * deltaX;
                CameraControls.selectionObj.transform.Translate(mOffset);
                transform.root.position = (CameraControls.selectionObj.transform.position);
                break;
            case 1:
                float deltaY = Input.GetAxis("Mouse Y") * (Time.deltaTime * 10 * distance);
                mOffset =Camera.main.transform.up * deltaY;
                CameraControls.selectionObj.transform.Translate(mOffset);
                transform.root.position=(CameraControls.selectionObj.transform.position);
                break;
            case 2:
                float deltaZ = Input.GetAxis("Mouse Y") * (Time.deltaTime*10 * distance);
                mOffset = Camera.main.transform.forward * deltaZ;
                CameraControls.selectionObj.transform.Translate(mOffset);
                transform.root.position = (CameraControls.selectionObj.transform.position);
                break;
        }
    }
}
