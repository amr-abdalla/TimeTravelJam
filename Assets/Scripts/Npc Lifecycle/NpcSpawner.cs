using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
	public static NpcSpawner Instance { get; private set; }
	[SerializeField] private GameObject NpcPrefab;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning("Two instances of InputManager detected, destroying one");
			Destroy(Instance);
		}
		Instance = this;
	}

	public async Awaitable<GameObject> SpawnDelayed(float delayInSeconds)
	{
		await Awaitable.WaitForSecondsAsync(delayInSeconds);
		return Spawn();
	}

	public GameObject Spawn() => Instantiate(NpcPrefab, null);
}
