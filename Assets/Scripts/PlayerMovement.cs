using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference sprintAction;
    [SerializeField] float cameraSpeed = 70f;
    [SerializeField] float cameraSprintSpeed = 140f;
    [SerializeField] Camera playerCamera;
    
    [Header("Zoom Settings")]
    [SerializeField] CinemachineCamera cinemachineCamera;
    [SerializeField] float zoomSpeed = 2f;
    [SerializeField] float minZoom = 8f;
    [SerializeField] float maxZoom = 15f;

    void Awake()
    {
        if (playerCamera == null) playerCamera = Camera.main;

        Cursor.visible = true;
    }

    void OnEnable()
    {
        moveAction.action.Enable();
        sprintAction.action.Enable();
    }
    
    void OnDisable()
    {
        moveAction.action.Disable();
        sprintAction.action.Disable();
    }

    void Update()
    {
        MovePlayerCamera();
        ZoomCamera();
    }

    void MovePlayerCamera()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        bool shiftPressed = sprintAction.action.IsPressed();
        Vector3 movement =  new Vector3(input.x, 0, input.y);

        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }
        if (!shiftPressed)
            transform.position += movement * cameraSpeed * Time.deltaTime;
        else
            transform.position += movement * cameraSprintSpeed * Time.deltaTime;
    }

    void ZoomCamera()
    {
        if (cinemachineCamera == null || Mouse.current == null) return;
        
        float scroll = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Approximately(scroll, 0f)) return;

        LensSettings lens = cinemachineCamera.Lens;
        lens.OrthographicSize = Mathf.Clamp(
            lens.OrthographicSize - scroll * zoomSpeed * Time.deltaTime,
            minZoom,
            maxZoom);
        
        cinemachineCamera.Lens = lens;
    }

}
