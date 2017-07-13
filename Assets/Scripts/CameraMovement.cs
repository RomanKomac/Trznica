#define MOBILE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class CameraMovement : MonoBehaviour {



    //Pinch zoom constants
    const float PERSPECTIVE_ZOOM_SPEED = 0.7f;
    const float DISTANCE_ZOOM_SPEED = 0.07f;
    const float MIN_CAMERA_DISTANCE = 2f;
    const float MAX_CAMERA_DISTANCE = 7f;
    const float MIN_PERSPECTIVE_SIZE = 15f;
    const float MAX_PERSPECTIVE_SIZE = 20f;

    //Container of numbers
    public Transform numberCont;

    //Pin
    public Transform pin;

    //Camera follow player constants
    const float CAMERA_FOLLOW_DAMPING = 1f;

    //Defines all the possible states of the camera
    enum CameraState { BASE, ROTATING, BIRDEYE }
    CameraState currentState = CameraState.BASE;

    public bool _isZemljevid { get; set; }
    private float touchTime = 0f;


    void Start()
    {
        foreach (Transform tf in numberCont)
        {
            if (tf.GetComponent<TextMesh>() != null)
                tf.GetComponent<TextMesh>().color = new Vector4(0, 0, 0, Mathf.Clamp((cameraPositonOffset.z - MIN_CAMERA_DISTANCE - 3.5f) / 3f, 0f, 1f));
        }
    }

    void Update() {
        if (!_isZemljevid) return;

        MoveCamera();

        transform.parent.eulerAngles = cameraRotationOffset;
        transform.localPosition = cameraPositonOffset;

        if (Input.touchCount == 2) {
            touchTime = 0;
            PinchZoom();
        }

#if MOBILE
        else if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
                touchTime += Time.deltaTime;
            else if(Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                if(touchTime > 0.5f)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    if (Physics.Raycast(ray, out hit))
                    {
                        pin.position = hit.point;
                        pin.position += new Vector3(0, 0.2f, 0);
                    }
                }
                touchTime = 0;
            }
        }
#else
        if (Input.GetMouseButton(0))
        {
            touchTime += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (touchTime > 0.5f)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    pin.position = hit.point;
                    pin.position += new Vector3(0, 0.2f, 0);
                }
            }
            touchTime = 0;
        }

#endif

    }

    public GameObject target;
    public Vector3 cameraPositonOffset = new Vector3(0, 0, 5f);
    public Vector3 cameraRotationOffset = new Vector3(-30, 0, 0);
    /// <summary>
    /// Makes the camera follow the users character and applies changes to camera position/rotation made by other functions.
    /// </summary>
    void MoveCamera() {
        if (target != null)
        {
            transform.parent.position = new Vector3(Mathf.Lerp(transform.parent.position.x, target.transform.parent.position.x, 0.02f), 0, Mathf.Lerp(transform.parent.position.z, target.transform.parent.position.z, 0.02f));
        }
        else {
            transform.parent.position = new Vector3(Mathf.Lerp(transform.parent.position.x, 0, 0.02f), 0, Mathf.Lerp(transform.parent.position.z, 0, 0.02f));
        }


    }

    public void SetTarget(GameObject target) {
        this.target = target;
    }

    /// <summary>
    /// Changes cameras field of view based on pinching on the screen.
    /// </summary>
    /// <param name="distanceZoomSpeed"> Optional parameter to set the speed of the distance of the camera changes
    void PinchZoom(float distanceZoomSpeed = DISTANCE_ZOOM_SPEED, float perspectiveZoomSpeed = PERSPECTIVE_ZOOM_SPEED) {
        
        // Store both touches.
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        // Find the position in the previous frame of each touch.
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // Find the magnitude of the vector (the distance) between the touches in each frame.
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // Find the difference in the distances between each frame.
        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        //When in BASE state, the camera zooms out at a 25 degree angle
        if (currentState == CameraState.BASE) {
            cameraPositonOffset.z += deltaMagnitudeDiff * distanceZoomSpeed;
            cameraPositonOffset.z = Mathf.Clamp(cameraPositonOffset.z, MIN_CAMERA_DISTANCE, MAX_CAMERA_DISTANCE);
            if (cameraPositonOffset.z == MAX_CAMERA_DISTANCE)
            {
                currentState = CameraState.ROTATING;
            }
            foreach (Transform tf in numberCont)
            {
                if(tf.GetComponent<TextMesh>()!=null)
                    tf.GetComponent<TextMesh>().color = new Vector4(0, 0, 0, Mathf.Clamp((cameraPositonOffset.z - MIN_CAMERA_DISTANCE-3.5f)/3f, 0f, 1f));
            }
            
        }
        //In the ROTATING state the camera rotates to a 90 degree angle
        else if (currentState == CameraState.ROTATING) {
            cameraRotationOffset.x -= deltaMagnitudeDiff * perspectiveZoomSpeed;
            cameraRotationOffset.x = Mathf.Max(cameraRotationOffset.x, -90);
            //transform.position = new Vector3((90f - cameraRotationOffset.x) / (60f) * transform.position.x, transform.position.y, (90f-cameraRotationOffset.x)/(60f)*transform.position.z);
            if (cameraRotationOffset.x == -90) {
                currentState = CameraState.ROTATING;
                
            }
            if (cameraRotationOffset.x > -30) {
                cameraRotationOffset.x = -30;
                currentState = CameraState.BASE;
            }
        }
    }
}