using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

public class PlayerInteract : MonoBehaviour, IInteractor
{
    [SerializeField] InputActionReference clickAction;
    [SerializeField] InputActionReference rightClickAction;
    [SerializeField] Camera cam;

    public event Action OnClickAway;
    
    public GameObject Owner => gameObject;
    public Transform Origin => transform;

    void Start()
    {
        if (cam == null) cam = Camera.main;
    }
    
    void OnEnable()
    {
        clickAction.action.Enable();
        rightClickAction.action.Enable();
    }
    
    void OnDisable()
    {
        clickAction.action.Disable();
        rightClickAction.action.Disable();
    }

    void Update()
    {
        DetectClick();
    }

    void DetectClick()
    {
        if (clickAction.action.WasPressedThisFrame())
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
            
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, -1))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                
                if (interactable != null) interactable.Interact(this, InteractionType.Primary);
                else OnClickAway?.Invoke();
            }
            else OnClickAway?.Invoke();
            
        }
        
        if (rightClickAction.action.WasPressedThisFrame())
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, -1))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                
                if (interactable != null) interactable.Interact(this, InteractionType.Secondary);
            }
        }
    }
}
