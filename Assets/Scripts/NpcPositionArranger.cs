using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class NpcPositionArranger : MonoBehaviour
{
	[SerializeField] private List<GameObject> npcs;
	[SerializeField] private int centerIndex = 0;
	[SerializeField] private float radius = 5f;
	[SerializeField] private float rotationSpeed = 2f;

	private float currentOffset;
	private bool initialized;

	private void OnEnable()
	{
		currentOffset = TargetOffset();
		initialized = true;
		ArrangeInCircle();
	}

	private void Update()
	{
		float target = TargetOffset();

		// Snap in edit mode / on the first frame; lerp during play.
		if (!initialized || !Application.isPlaying || rotationSpeed <= 0f)
		{
			currentOffset = target;
			initialized = true;
		}
		else
		{
			// Frame-rate independent easing: same settle time at 30 or 300 fps.
			float t = 1f - Mathf.Exp(-rotationSpeed * Time.deltaTime);
			currentOffset = Mathf.LerpAngle(currentOffset, target, t);

			if (Mathf.Abs(Mathf.DeltaAngle(currentOffset, target)) < 0.01f)
				currentOffset = target;
		}

		ArrangeInCircle();
	}

	private float TargetOffset()
	{
		if (npcs == null || npcs.Count == 0)
			return 0f;

		int center = Mathf.Clamp(centerIndex, 0, npcs.Count - 1);
		return - center * 360f / npcs.Count;
	}

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
		if (npcs == null || index < 0 || index > npcs.Count - 1)
			return;

		centerIndex = index;
	}

	public void Next() => SetCenterIndex(centerIndex + 1);
	public void Previous() => SetCenterIndex(centerIndex - 1);

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}