using UnityEngine;

public class SelectionIndicator : MonoBehaviour {

	MouseManager mm;
	void Start () {
		mm = GetComponent<MouseManager>();
	}
	
	void Update () {

		if(mm.selectedObject != null) {
			GetComponentInChildren<Renderer>().enabled = true;
			Bounds bigBounds = mm.selectedObject.GetComponentInChildren<Renderer>().bounds;
			float padding = 1.0f;
			this.transform.position = new Vector3(bigBounds.center.x, bigBounds.center.y, bigBounds.center.z);
			this.transform.localScale = new Vector3( bigBounds.size.x*padding, bigBounds.size.y*padding, bigBounds.size.z*padding );
		}
		else {
			GetComponentInChildren<Renderer>().enabled = false;
		}
	}
}
 