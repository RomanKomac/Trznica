using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    //Pinch zoom constants
    const float PERSPECTIVE_ZOOM_SPEED = 0.15f;
    const float DISTANCE_ZOOM_SPEED = 0.15f;
    const float MIN_CAMERA_DISTANCE = 5f;
    const float MAX_CAMERA_DISTANCE = 10f;
    const float MIN_PERSPECTIVE_SIZE = 15f;
    const float MAX_PERSPECTIVE_SIZE = 30f;

    //Camera follow player constants
    const float CAMERA_FOLLOW_DAMPING = 1f;

    //Defines all the possible states of the camera
    enum CameraState { BASE, ROTATING, BIRDEYE }
    CameraState currentState = CameraState.BASE;

    public bool _isZemljevid { get; set; }

    void Update() {
        if (!_isZemljevid) return;

        if (Input.touchCount == 2) {
            MoveCamera();
            PinchZoom();
        }
    }

    public GameObject target;
    public Vector3 cameraPositonOffset = new Vector3(0, 0, 5f);
    public Vector3 cameraRotationOffset = new Vector3(30, 0, 0);
    /// <summary>
    /// Makes the camera follow the users character and applies changes to camera position/rotation made by other functions.
    /// </summary>
    void MoveCamera() {
        //Get current camera angle
        float currentAngle = transform.eulerAngles.y;
        //Get the angle of our users character
        float desiredAngle = target.transform.eulerAngles.y;
        //Slowly adjust the camera angle towards the user characters angle
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * CAMERA_FOLLOW_DAMPING);

        //Calculate the Quaternion for camera
        Quaternion rotation = Quaternion.Euler(cameraRotationOffset.x, angle, cameraRotationOffset.z);
        //Sets the new camera position adjusted for the rotation
        transform.position = target.transform.position - (rotation * cameraPositonOffset);
        //Sets the new camera rotation
        transform.rotation = rotation;
    }

    public void SetTarget(GameObject target) {
        this.target = target;
    }

    /// <summary>
    /// Changes cameras field of view based on pinching on the screen.
    /// </summary>
    /// <param name="distanceZoomSpeed"> Optional parameter to set the speed of the distance of the camera changes
    void PinchZoom(float distanceZoomSpeed = DISTANCE_ZOOM_SPEED) {
        
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
            cameraPositonOffset.z = Mathf.Clamp(cameraPositonOffset.z, 5, 20);
            if (cameraPositonOffset.z == 20) {
                currentState = CameraState.ROTATING;
            }
        }
        //In the ROTATING state the camera rotates to a 90 degree angle
        else if (currentState == CameraState.ROTATING) {
            cameraRotationOffset.x += deltaMagnitudeDiff * distanceZoomSpeed;
            cameraRotationOffset.x = Mathf.Min(cameraRotationOffset.x, 90);
            if (cameraRotationOffset.x == 90) {
                currentState = CameraState.ROTATING;
            }
            if (cameraRotationOffset.x < 30) {
                cameraRotationOffset.x = 30;
                currentState = CameraState.BASE;
            }
        }
    }
}