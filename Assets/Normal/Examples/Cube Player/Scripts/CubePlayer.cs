using UnityEngine;
using Normal.Realtime;

namespace Normal.Realtime.Examples {
    public class CubePlayer : MonoBehaviour {
        public float speed = 5.0f;

        private RealtimeView      _realtimeView;
        private RealtimeTransform _realtimeTransform;

        // Main Camera Offset
        private float offsetx = 0f;
        private float offsety = 5f;
        private float offsetz = -10f;

        GameObject m_MainCamera;
        GameObject m_Canvas;
        GameObject m_EventSystem;

        // Movement Sensitivity Settings
        float mainSpeed = 10.0f; //regular speed
        float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
        float maxShift = 500.0f; //Maximum speed when holdin gshift
        float camSens = 0.25f; //How sensitive it with mouse

        private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
        private float totalRun= 1.0f;

        private void Awake() {
            _realtimeView      = GetComponent<RealtimeView>();
            _realtimeTransform = GetComponent<RealtimeTransform>();
        }

        private bool areObjectsParented = false;

        private void Update() {
            // If this CubePlayer prefab is not owned by this client, bail.
            if (!_realtimeView.isOwnedLocally)
                return;

            // Move the camera
            if (!areObjectsParented) {
                m_MainCamera = Camera.main.gameObject;
                m_MainCamera.transform.parent = transform;
                Vector3 offset = new Vector3(offsetx, offsety, offsetz);
                m_MainCamera.transform.position = transform.position + offset;
                m_MainCamera.transform.LookAt(transform);

                m_Canvas = GameObject.Find("PlayerCanvas");
                m_EventSystem = GameObject.Find("PlayerEventSystem");
                m_Canvas.transform.parent = transform;
                m_EventSystem.transform.parent = transform;

                areObjectsParented = true;
            }

            // Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
            _realtimeTransform.RequestOwnership();

            // Movement Script
            lastMouse = Input.mousePosition - lastMouse ;
            lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0 );
            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x , transform.eulerAngles.y + lastMouse.y, 0);
            transform.eulerAngles = lastMouse;
            lastMouse =  Input.mousePosition;
            //Mouse  camera angle done.

            //Keyboard commands
            Vector3 p = GetBaseInput();
            if (Input.GetKey (KeyCode.LeftShift)){
                totalRun += Time.deltaTime;
                p  = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            }
            else{
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }

            p = p * Time.deltaTime;

            Vector3 newPosition = transform.position;

            transform.Translate(p);

        }

        private Vector3 GetBaseInput() { //returns the basic values, if it's 0 than it's not active.
            Vector3 p_Velocity = new Vector3();
            if (Input.GetKey (KeyCode.W)){
                p_Velocity += new Vector3(0, 0 , 1);
            }
            if (Input.GetKey (KeyCode.S)){
                p_Velocity += new Vector3(0, 0, -1);
            }
            if (Input.GetKey (KeyCode.A)){
                p_Velocity += new Vector3(-1, 0, 0);
            }
            if (Input.GetKey (KeyCode.D)){
                p_Velocity += new Vector3(1, 0, 0);
            }
            if (Input.GetKey (KeyCode.Space)){
                p_Velocity += new Vector3(0, 2, 0);
            }
            return p_Velocity;
        }
    }
}
