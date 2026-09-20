using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
	public static ItemDatabase Instance { get; private set; }
	public TimeItem[] AllItems { get {  return allItems; } }
	[SerializeField] private TimeItem[] allItems;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning($"Two instances of {GetType()} detected, destroying one");
			Destroy(Instance);
		}
		Instance = this;
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public TimeItem GetRandomItem()
	{
		if (allItems == null || allItems.Length == 0)
		{
			Debug.LogError($"{GetType()} has no items assigned");
			return null;
		}

		return allItems[Random.Range(0, allItems.Length)];
	}

	private static readonly TimePeriod[] periods = (TimePeriod[])System.Enum.GetValues(typeof(TimePeriod));

	public TimePeriod GetRandomPeriod()
	{
		return periods[Random.Range(0, periods.Length)];
	}
}
