using UnityEngine;

public class CameraControls : MonoBehaviour
{
	const string INPUT_MOUSE_SCROLLWHEEL = "Mouse ScrollWheel";
	const string INPUT_MOUSE_X = "Mouse X";
	const string INPUT_MOUSE_Y = "Mouse Y";
	const float MIN_CAM_DISTANCE = 1f;
	const float MAX_CAM_DISTANCE = 30f;
    float distance = 0f;

    public GameObject Gizmo;
    RaycastHit hit;
    int Mask;//9obj 10 ui

    //할당되어지지않는것들
    public int NoSelection = 0;
    public GameObject selectionObj;
    public GameObject TargetObj1;
    public GameObject TargetObj2;

    [Range(2f, 15f)]
	public float orbitSpeed = 6f;
    
	[Range(.3f,2f)]
	public float zoomSpeed = .8f;
    

	void Start()
	{
		distance = Vector3.Distance(transform.position, Vector3.zero);
	}

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //부딪히는 위치기준으로 위치의 좌교값을 받아서 해주고있늗데 문제는 이게 y좌표값
            //이나 z좌표값이 정확하지 않아서 
            if (Physics.Raycast(ray, out hit, 100, Mask = 1 << 9))
            {
                selectionObj = hit.collider.gameObject;
                selectionObj.GetComponent<Collider>().enabled = false;
                NoSelection = 1;

                Gizmo.SetActive(true);
                Gizmo.transform.position = hit.transform.position;
                Gizmo.transform.SetParent(hit.transform);
            }
            //실행 순서대로 생각했
            else if (Physics.Raycast(ray, out hit, 100, Mask = 1 << 10))
            {
                selectionObj = hit.collider.gameObject;
                selectionObj.GetComponent<Collider>().enabled = false;
                NoSelection = 2;
                selectionObj.transform.position = hit.point;
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectionObj)
                selectionObj.GetComponent<Collider>().enabled = true;

            Mask = 1 << 10;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, Mask))
            {
                hit.collider.transform.GetComponent<MeshFilter>().mesh =
                    selectionObj.GetComponent<MeshFilter>().mesh;
                if (NoSelection == 1)
                {
                    Gizmo.transform.parent = null;
                    Gizmo.SetActive(false);
                    Destroy(selectionObj);
                }
                else if (NoSelection == 2)
                {
                    int num = selectionObj.GetComponent<MeshFilter>().mesh.vertices.Length;
                    if (num > 1)
                    {
                        selectionObj.transform.position = hit.point;
                        GameObject Gma = Instantiate<GameObject>(selectionObj,
                        selectionObj.transform.position, transform.rotation);
                        Gma.layer = 9;
                        hit.collider.transform.localPosition = Vector3.zero;
                        hit.collider.transform.GetComponent<MeshFilter>().mesh = null;
                    }
                }
            }
            NoSelection = 0;
        }
        
        if (Input.GetMouseButton(1) && NoSelection == 0)
        {
            float rot_x = Input.GetAxis(INPUT_MOUSE_X);
            float rot_y = -Input.GetAxis(INPUT_MOUSE_Y);

            Vector3 eulerRotation = transform.localRotation.eulerAngles;

            eulerRotation.x += rot_y * orbitSpeed;
            eulerRotation.y += rot_x * orbitSpeed;
            eulerRotation.z = 0f;

            transform.localRotation = Quaternion.Euler(eulerRotation);
            transform.position = transform.localRotation * (Vector3.forward * -distance);
        }

        if ( Input.GetAxis(INPUT_MOUSE_SCROLLWHEEL) != 0f )
		{
			float delta = Input.GetAxis(INPUT_MOUSE_SCROLLWHEEL);
			distance -= delta * (distance/MAX_CAM_DISTANCE) * (zoomSpeed * 1000) * Time.deltaTime;
            //          스크롤값* 거리/ 카메라 최대길이 * 줌스피드*1000 
            distance = Mathf.Clamp(distance, MIN_CAM_DISTANCE, MAX_CAM_DISTANCE);
			transform.position = transform.localRotation * (Vector3.forward * -distance);
            //                              로컬회전 벡터     0,0,1          
        }
        if (Input.GetMouseButtonDown(2))
        {
             Mask = 1 << 9;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, Mask))
            {
                if (TargetObj1 == hit.collider.gameObject)
                {
                    TargetObj1.GetComponent<Renderer>().material.color = Color.white;
                    TargetObj1 = null;
                }

                else if (TargetObj2 == hit.collider.gameObject)
                {
                    TargetObj2.GetComponent<Renderer>().material.color = Color.white;
                    TargetObj2 = null;
                }
                else
                {
                    if (!TargetObj1)
                    {
                        TargetObj2 = hit.collider.gameObject;
                        TargetObj2.GetComponent<Renderer>().material.color = Color.red;
                        Vector3 ve3 = TargetObj2.GetComponent<Renderer>().bounds.size;
                        TargetObj1 = TargetObj2;
                        TargetObj2 = null;
                        
                    }
                    else if (!TargetObj2)
                    {
                        TargetObj2 = hit.collider.gameObject;
                        TargetObj2.GetComponent<Renderer>().material.color = Color.red;
                    }
                    else 
                    {
                        TargetObj1.GetComponent<Renderer>().material.color = Color.white;
                        TargetObj1 = TargetObj2;
                        TargetObj2 = hit.collider.gameObject;
                        TargetObj2.GetComponent<Renderer>().material.color = Color.red;
                    }
                }
            }
        }
    }

}
