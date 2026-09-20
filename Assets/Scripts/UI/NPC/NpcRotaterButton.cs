using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class NpcRotaterButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] private int pos = 1;
	[SerializeField] private float degreesPerSecond = 120f;

	private Transform target;

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log("down");
		if (eventData.button != PointerEventData.InputButton.Left)
			return;

		GameObject centered = NpcPositionManager.Instance != null
			? NpcPositionManager.Instance.GetSelectedNPC()
			: null;

		target = centered != null ? centered.transform : null;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
			target = null;
	}

	private void OnDisable()
	{
		target = null;
	}

	private void Update()
	{
		if (target == null)
			return;

		if (Mouse.current != null && !Mouse.current.leftButton.isPressed && Touchscreen.current == null)
		{
			target = null;
			return;
		}

		target.Rotate(0f, pos * degreesPerSecond * Time.deltaTime, 0f, Space.World);
	}
}