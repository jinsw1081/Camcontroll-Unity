
using UnityEngine;

public class Objects : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    public float explosionRadius = 5.0f;
   
    void OnMouseDown()
    {
        
        mZCoord = Camera.main.WorldToScreenPoint(
          gameObject.transform.position).z;

        // Store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseAsWorldPointZ();
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
        transform.position = GetMouseAsWorldPointZ() + mOffset;

    }
    //마우스 좌표는 카메라 좌표를 반환하고
    //마우스좌표를 월드스페이스로 바꾼다음에 클릭한다음 처음의 위치와 차이를 빼서 뺀만큼을
    //차이를 오브젝트 위치에서 빼주면은 됨
    //그러면 y축은 xy로만 
}
