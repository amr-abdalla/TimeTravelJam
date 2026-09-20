using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
	public bool InteractionEnabled = true;

	[SerializeField] private UnityEvent _interactionEvent;
	[SerializeField] private UnityEvent _mouseHoverEvent;
	[SerializeField] private UnityEvent _mouseExitEvent;

	protected Outline _outline;

	protected void Awake()
	{
		if (!this.CompareTag("Interactable"))
			Debug.LogError($"Object {this.gameObject.name} is not tagged Interactable, interaction script won't work", this.gameObject);

		_outline = GetComponent<Outline>();
		_outline.enabled = false;
	}

	public virtual void Interaction()
	{
		_interactionEvent.Invoke();
		AudioManager.Instance.Play("click");
		Debug.Log("Interaction", this.gameObject);
	}

	public virtual void MouseHoverCallback()
	{
		if (!InteractionEnabled)
			return;

		if(gameObject.activeSelf)
			AudioManager.Instance.Play("hover");


		_outline.enabled = true;

		_mouseHoverEvent.Invoke();
	}

	public virtual void MouseExitCallback()
	{
		if (!InteractionEnabled)
			return;

		if (gameObject.activeSelf)
			AudioManager.Instance.Play("hover");

		_outline.enabled = false;

		_mouseExitEvent.Invoke();
	}
}