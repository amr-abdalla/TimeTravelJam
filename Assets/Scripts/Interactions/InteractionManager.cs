using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
	public static InteractionManager Instance { get; private set; }

	public Vector2 MousePosition {  get; private set; } //if for any reasons we need to gather the mouse pos outside of the input manager (panning the camera ?)

	private InputSystem_Actions _actions;
	private Camera _cam;

	private Interactable _currentInteractable;

	private void Awake()
	{
		if (Instance != null) {
			Debug.LogWarning("Two instances of InputManager detected, destroying one");
			Destroy(Instance); 
		}
		Instance = this;

		_actions = new InputSystem_Actions();

		_actions.Player.MousePosition.performed += ctx => mousePositionCallback(ctx);
		_actions.Player.MouseInteraction.performed += ctx => mouseClickCallback();

		_cam = Camera.main;
		if (_cam == null) _cam = FindFirstObjectByType<Camera>();
		_currentInteractable = null;
	}

	private void OnEnable()
	{
		_actions.Enable();
	}

	private void OnDisable()
	{
		_actions.Disable();
	}

	private void mousePositionCallback(InputAction.CallbackContext ctx)
	{
		MousePosition = ctx.ReadValue<Vector2>();
	}

	private void mouseClickCallback()
	{
		if (_currentInteractable == null)
			return;

		if(_currentInteractable.InteractionEnabled)
			_currentInteractable.Interaction();
	}

	private void Update()
	{
		RaycastHit hit;
		if (Physics.Raycast(_cam.ScreenPointToRay(MousePosition), out hit))
		{
			if (hit.collider.CompareTag("Interactable"))
			{
				Interactable interact = hit.collider.gameObject.GetComponent<Interactable>();

				if (interact == null)
				{
					Debug.LogError($"Object {hit.collider.gameObject} has been tagged Interactable but does'nt contain any script that derives from Interactable base class");
					return;
				}

				if (interact != _currentInteractable && interact.enabled)
				{
					_currentInteractable = interact;
					_currentInteractable.MouseHoverCallback();
				}
			}
			else if (_currentInteractable != null && _currentInteractable.enabled)
				cleanCurrentInteract();
		}
		else if (_currentInteractable != null && _currentInteractable.enabled)
			cleanCurrentInteract();
	}

	private void cleanCurrentInteract()
	{
		_currentInteractable.MouseExitCallback();
		_currentInteractable = null;
	}
}