using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPointer : MonoBehaviour
{
    public float maxDistance = 30.0f;
    private GameObject gazedAtObject = null;

    [Header("Simulador de ratón en PC")]
    public float mouseSensitivity = 0.1f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private UnityEngine.SpatialTracking.TrackedPoseDriver poseDriver;

    void Start()
    {
        poseDriver = GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>();

        if (Application.isEditor && poseDriver != null)
        {
            poseDriver.enabled = false;
        }
    }

    void Update()
    {
        if (Application.isEditor && Mouse.current != null)
        {
            bool isRightClick = Mouse.current.rightButton.isPressed;
            bool isAltPressed = Keyboard.current != null && Keyboard.current.leftAltKey.isPressed;

            if (isRightClick || isAltPressed)
            {
                Vector2 mouseDelta = Mouse.current.delta.ReadValue();
                yaw += mouseDelta.x * mouseSensitivity;
                pitch -= mouseDelta.y * mouseSensitivity;
                pitch = Mathf.Clamp(pitch, -85f, 85f);
                transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
            }
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            if (gazedAtObject != hit.transform.gameObject)
            {
                if (gazedAtObject != null)
                {
                    gazedAtObject.SendMessage("OnPointerExit", SendMessageOptions.DontRequireReceiver);
                }

                gazedAtObject = hit.transform.gameObject;
                gazedAtObject.SendMessage("OnPointerEnter", SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            if (gazedAtObject != null)
            {
                gazedAtObject.SendMessage("OnPointerExit", SendMessageOptions.DontRequireReceiver);
                gazedAtObject = null;
            }
        }
    }
}