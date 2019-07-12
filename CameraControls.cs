using UnityEngine;

public class CameraControls : MonoBehaviour
{
	const string INPUT_MOUSE_SCROLLWHEEL = "Mouse ScrollWheel";
	const string INPUT_MOUSE_X = "Mouse X";
	const string INPUT_MOUSE_Y = "Mouse Y";
	const float MIN_CAM_DISTANCE = 1f;
	const float MAX_CAM_DISTANCE =10f;

    public GameObject Gizmo;
    public GameObject bumpedUi;
    BumpedObjectScript bumpedObjectScript;

    RaycastHit hit;
    int Mask;//9obj 10 ui

    //할당되어지지않는것들
    public int NoSelection = 0;

    public static GameObject selectionObj;
    public static GameObject TargetObj1;
    public static GameObject TargetObj2;
    public static bool gizmoMoveOn=false;

    [Range(1f, 10f)]
	public float orbitSpeed = 1f;
	[Range(.3f,2f)]
	public float zoomSpeed = .8f;


	void Start()
	{
        bumpedObjectScript = bumpedUi.GetComponent<BumpedObjectScript>();
	}

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //부딪히는 위치기준으로 위치의 좌교값을 받아서 해주고있늗데 문제는 이게 y좌표값
            //이나 z좌표값이 정확하지 않아서 

            if (Physics.Raycast(ray, out hit, 100, Mask = 1 << 11))
            {
                //효과없음
                gizmoMoveOn = true;
            }
            else if (Physics.Raycast(ray, out hit, 100, Mask = 1 << 9))
            {
                selectionObj = hit.collider.gameObject;
                //selectionObj.GetComponent<Collider>().enabled = false;
                NoSelection = 1;
                if (selectionObj.tag != "others")
                    bumpedObjectScript.SetText();
                Gizmo.SetActive(true);
                Gizmo.transform.position = hit.transform.position;
                gizmoMoveOn = false;
                // Gizmo.transform.SetParent(hit.transform);
            }

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
                if (selectionObj)
                {
                    if (selectionObj.tag != "others")
                    {
                        hit.collider.transform.GetComponent<MeshFilter>().mesh =
                        selectionObj.GetComponent<MeshFilter>().mesh;
                        
                        if (NoSelection == 1)
                        {
                            hit.collider.transform.name = selectionObj.transform.name;
                            Gizmo.SetActive(false);
                            Destroy(selectionObj);

                        }
                        else if (NoSelection == 2)
                        {
                            //To do 수정해야하는 부분 오브젝트 새로 만드는 부분과 
                            int num = selectionObj.GetComponent<MeshFilter>().mesh.vertices.Length;
                            if (num > 1)
                            {
                                selectionObj.transform.position = hit.point;
                                Gizmo.transform.position = hit.point;
                                GameObject Gma = Instantiate<GameObject>(selectionObj,
                                selectionObj.transform.position, transform.rotation);
                                string name = hit.collider.transform.name;
                                hit.collider.transform.name = null;
                                Gma.name = name;        //이름에 clone안생기게하는부분;
                                if (Gma.GetComponent<Collider>())
                                    Destroy(Gma.GetComponent<Collider>());
                                Gma.AddComponent<MeshCollider>();

                                Gma.layer = 9;
                                hit.collider.transform.localPosition = Vector3.zero;
                                hit.collider.transform.GetComponent<MeshFilter>().mesh = null;
                            }
                        }
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
            //eulerRotation.z = 0f;
            transform.localRotation = Quaternion.Euler(eulerRotation);
            Gizmo.transform.localRotation= Quaternion.Euler(eulerRotation);
            //  bumpedUi.transform.rotation= Quaternion.Euler(eulerRotation);
            //  transform.position = transform.localRotation * (Vector3.forward * -distance);\
            
        }

        if ( Input.GetAxis(INPUT_MOUSE_SCROLLWHEEL) != 0f )
		{
			float delta = Input.GetAxis(INPUT_MOUSE_SCROLLWHEEL);
            if(delta>0)
            transform.position += transform.forward ;
            else
                transform.position -= transform.forward * 2;

            float localScaleX = transform.localScale.x;
            //if(localScaleX<0)
            //그러니깐 1,1,1, 이기본인데 문제는 1,1,1에서 줄어들때 스케일이 -1로 가버리면은 자체가 뒤집혀버린다
            //뒤집히는 걸 방지하기 위해서 
            //0이하로는 가지않고 0~1 까지는 작아질수록 적게 줄어들고 1부터는 상대적으로 많이 커지는 
            //마우스로 움직이는 델타값이있고 -1~1 의 값을 반환한다.
         
            float R=1;
            if (delta < 0)
                R = -1;
            Gizmo.transform.localScale +=R* delta*delta* Vector3.one;
        }
        if(Input.GetMouseButton(2))
        {
            float _x = Input.GetAxis("Mouse X");
            float _y = Input.GetAxis("Mouse Y");

            Vector3 curPos = transform.position;
            if (Input.GetAxis("Mouse X") > 0)
                curPos = curPos - transform.right / 20;
            else if (Input.GetAxis("Mouse X") < 0)
                curPos = curPos + transform.right/20;
            if (Input.GetAxis("Mouse Y") > 0)
                curPos = curPos - transform.up / 20;
            else if (Input.GetAxis("Mouse Y") < 0)
                curPos = curPos + transform.up / 20;
            transform.position = curPos;
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
