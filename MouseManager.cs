using UnityEngine;

public class MouseManager : MonoBehaviour {
    //마우스관련 셀렉션 박스 
 	public static GameObject selectedObject;
    public static GameObject notChangedObject;
    CameraControls camCon;
    Ray ray;
    RaycastHit hitInfo;
    int Mask;

    private void Start()
    {
        camCon = Camera.main.GetComponent<CameraControls>();
    }

    void LateUpdate() {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Mask = 1 << 9;
        if (Physics.Raycast(ray, out hitInfo, 100, Mask)) {
            
            GameObject hitObject = hitInfo.collider.gameObject;
            if(hitInfo.collider.tag!="others"&&Input.GetMouseButtonDown(0))
            notChangedObject= hitInfo.collider.gameObject;
            SelectObject(hitObject);
        }
       
        else {
			ClearSelection();
		}
	
	}
    
	void SelectObject(GameObject obj) {
		if(selectedObject != null) {
			if(obj == selectedObject)
				return;

			ClearSelection();
		}
		selectedObject = obj;
	}

	void ClearSelection() {
		if(selectedObject == null)
			return;
        
		selectedObject = null;
    }
}
