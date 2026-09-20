using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcPositionManager : MonoBehaviour
{
	public static NpcPositionManager Instance { get; private set; }

	[SerializeField] private List<GameObject> npcs;
	[SerializeField] private int selectedIndex = 0;
	[SerializeField] private float radius = 5f;
	[SerializeField] private float rotationSpeed = 2f;
	[SerializeField] private NpcText npcText;

	private float currentOffset;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning($"Two instances of {GetType()} detected, destroying one");
			Destroy(Instance);
		}
		Instance = this;
	}

	private void OnEnable()
	{
		SetCenterIndex(0);
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
		}

		ArrangeInCircle();
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

			float angle = (
				i * 360f / numberOfObjects + currentOffset
			) * Mathf.Deg2Rad;

			Vector3 localPosition = new Vector3(
				Mathf.Cos(angle) * radius,
				0f,
				Mathf.Sin(angle) * radius
			);

			obj.transform.position = transform.TransformPoint(localPosition);
		}
	}

	public void SetCenterIndex(int index)
	{
		if (npcs == null || npcs.Count <= 0)
			return;

		OnDeselectNPC(GetSelectedNPC());

		if (index < 0)
		{
			index = npcs.Count - 1;
		}
		else
		{
			index = index % npcs.Count;
		}

		selectedIndex = index;
		OnSelectNPC(GetSelectedNPC());
	}

	private void OnDeselectNPC(GameObject npc)
	{
		npcText.gameObject.SetActive(false);
		npc.GetComponent<Interactable>().InteractionEnabled = true;
		npc.transform.rotation = Quaternion.Euler(0, 180, 0);
		npc.GetComponent<InteractableNPC>().InteractionEnabled = true;
		npc.GetComponent<Collider>().enabled = true;

		Interactable[] interactables = npc.GetComponentsInChildren<Interactable>().Where(c => c.gameObject != npc.gameObject).ToArray();
		foreach (Interactable interactable in interactables)
		{
			interactable.InteractionEnabled = false;
		}
	}

	private void OnSelectNPC(GameObject npc)
	{
		npc.GetComponent<Interactable>().InteractionEnabled = false;
		npc.GetComponent<Outline>().enabled = false;
		npc.GetComponent<InteractableNPC>().InteractionEnabled = false;
		npc.GetComponent<Collider>().enabled = false;

		npcText.UpdateText(npc.GetComponent<NpcData>());
		npcText.gameObject.SetActive(true);

		Interactable[] interactables = npc.GetComponentsInChildren<Interactable>().Where(c => c.gameObject != npc.gameObject).ToArray();
		foreach (Interactable interactable in interactables)
		{
			interactable.InteractionEnabled = true;
		}
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

	public async void RemoveAndDestroyCurrent()
	{
		GameObject selected = GetSelectedNPC();

		TimePeriod timePeriod = selected.GetComponent<NpcData>().GetGoalTimePeriod();
		DressUpCharacter dressUpCharacter = selected.GetComponent<DressUpCharacter>();
		StabilityManager.Instance.SendSomeoneThroughTime(dressUpCharacter, timePeriod);

		Destroy(selected);
		int destroyedIndex = selectedIndex;
		SetCenterIndex(selectedIndex + 1);
		npcs[destroyedIndex] = await NpcSpawner.Instance.SpawnDelayed(2f);
	}

	public void RemoveNPC(GameObject npc) => npcs.Remove(npc);

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}