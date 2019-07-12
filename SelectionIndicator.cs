using UnityEngine;
using UnityEngine.UI;

public class SelectionIndicator : MonoBehaviour {

	public GameObject bumpedUi;

    void LateUpdate () {
		if(MouseManager.selectedObject != null) {
			GetComponentInChildren<Renderer>().enabled = true;
			Bounds bigBounds = MouseManager.selectedObject.GetComponentInChildren<Renderer>().bounds;
			float padding = 1.0f;
			this.transform.position = new Vector3(bigBounds.center.x, bigBounds.center.y, bigBounds.center.z);
			this.transform.localScale = new Vector3( bigBounds.size.x*padding, 
                bigBounds.size.y*padding, bigBounds.size.z*padding );
          

        }
		else {
			GetComponentInChildren<Renderer>().enabled = false;
           
        }
        if(MouseManager.notChangedObject)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(MouseManager.notChangedObject.
                transform.position);
            //Vector3 boundInterval=MouseManager.notChangedObject.GetComponent<Collider>().bounds.size*50;

            //screenPos=screenPos+ boundInterval;
       //   screenPos = screenPos + new Vector3(150,0,0);
            bumpedUi.transform.position = screenPos;
        }

      
    }
}
 