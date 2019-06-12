using UnityEngine;

public class MouseManager : MonoBehaviour {
    public static int MouseMod=2;  //0:: z  1::y 2::x
	public GameObject selectedObject;
    Ray ray;
    RaycastHit hitInfo;
    int Mask;

    void LateUpdate() {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Mask = 1 << 9;
        if (Physics.Raycast(ray, out hitInfo, 100, Mask)) {

            GameObject hitObject = hitInfo.collider.gameObject;
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
