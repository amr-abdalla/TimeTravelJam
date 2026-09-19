using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
	public bool InteractionEnabled = true;

	[SerializeField] private UnityEvent _interactionEvent;
	[SerializeField] private UnityEvent _mouseHoverEvent;
	[SerializeField] private UnityEvent _mouseExitEvent;

	private Outline _outline;

	protected void Awake()
	{
		if (!this.CompareTag("Interactable"))
			Debug.LogError($"Object {this.gameObject.name} is not tagged Interactable, interaction script won't work", this.gameObject);

		_outline = GetComponent<Outline>();
		_outline.enabled = false;
	}

	public void Interaction()
	{
		_interactionEvent.Invoke();
		Debug.Log("Interaction", this.gameObject);
	}

	public void MouseHoverCallback()
	{
		if (!InteractionEnabled)
			return;

		_outline.enabled = true;

		_mouseHoverEvent.Invoke();
	}

	public void MouseExitCallback()
	{
		if (!InteractionEnabled)
			return;

		_outline.enabled = false;

		_mouseExitEvent.Invoke();
	}
}