using System.Linq;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
	public static NpcSpawner Instance { get; private set; }
	[SerializeField] private GameObject NpcPrefab;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning($"Two instances of {GetType()} detected, destroying one");
			Destroy(Instance);
		}
		Instance = this;
	}

	public async Awaitable<GameObject> SpawnDelayed(float delayInSeconds)
	{
		await Awaitable.WaitForSecondsAsync(delayInSeconds);
		return Spawn();
	}

	public GameObject Spawn()
	{
		GameObject npc = Instantiate(NpcPrefab, null);
		// some cool vfx here
		//npc.SetActive(false);

		npc.GetComponent<NpcData>().Randomize();
		Interactable[] interactables = npc.GetComponentsInChildren<Interactable>().Where(c => c.gameObject != npc.gameObject).ToArray();
		foreach (Interactable interactable in interactables)
		{
			interactable.InteractionEnabled = false;
		}

		//npc.SetActive(true);
		return npc;
	}
}
