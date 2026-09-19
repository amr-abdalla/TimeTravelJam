using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NpcPositionManager : MonoBehaviour
{
	public static NpcPositionManager Instance { get; private set; }

	[SerializeField] private List<GameObject> npcs;
	[SerializeField] private int selectedIndex = 0;
	[SerializeField] private float radius = 5f;
	[SerializeField] private float rotationSpeed = 2f;

	private float currentOffset;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning("Two instances of InputManager detected, destroying one");
			Destroy(Instance);
		}
		Instance = this;
	}

	private void OnEnable()
	{
		currentOffset = TargetOffset();
		ArrangeInCircle();
	}

	private void Update()
	{
		float target = TargetOffset();

		// Frame-rate independent easing: same settle time at 30 or 300 fps.
		float t = 1f - Mathf.Exp(-rotationSpeed * Time.deltaTime);
		currentOffset = Mathf.LerpAngle(currentOffset, target, t);

		if (Mathf.Abs(Mathf.DeltaAngle(currentOffset, target)) < 0.01f)
		{
			currentOffset = target;
			ArrangeInCircle();
		}

		if (currentOffset != target)
		{
			ArrangeInCircle();
		}
	}

	private float TargetOffset()
	{
		if (npcs == null || npcs.Count == 0)
			return 0f;

		int center = Mathf.Clamp(selectedIndex, 0, npcs.Count - 1);
		return -center * 360f / npcs.Count;
	}

	public GameObject GetSelectedNPC() => npcs[selectedIndex];

	public void ArrangeInCircle()
	{
		if (npcs == null || npcs.Count == 0)
			return;

		int numberOfObjects = npcs.Count;
		for (int i = 0; i < numberOfObjects; i++)
		{
			GameObject obj = npcs[i];
			if (obj == null)
				continue;

			float angle = (i * 360f / numberOfObjects + currentOffset) * Mathf.Deg2Rad;

			float x = Mathf.Cos(angle) * radius;
			float z = Mathf.Sin(angle) * radius;

			obj.transform.position = new Vector3(x, 0, z) + transform.position;
		}
	}

	public void SetCenterIndex(int index)
	{
		if (npcs == null || npcs.Count <= 0)
			return;

		GetSelectedNPC().GetComponent<Interactable>().InteractionEnabled = true;
		GetSelectedNPC().transform.rotation = Quaternion.Euler(0, 90, 0);

		if (index < 0)
		{
			index = npcs.Count - 1;
		}
		else
		{
			index = index % npcs.Count;
		}

		selectedIndex = index;
		GetSelectedNPC().GetComponent<Interactable>().InteractionEnabled = false;
		GetSelectedNPC().GetComponent<Outline>().enabled = false;
	}

	public void SelectNPC(GameObject npc)
	{
		for (int i = 0; i < npcs.Count; i++)
		{
			if (npcs[i] == npc)
			{
				SetCenterIndex(i);
				return;
			}
		}
	}

	public void RemoveAndDestroyCurrent(InputAction.CallbackContext _)
	{
		GameObject selected = GetSelectedNPC();
		RemoveNPC(selected);
		Destroy(selected);
	}

	public void RemoveNPC(GameObject npc) => npcs.Remove(npc);

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}