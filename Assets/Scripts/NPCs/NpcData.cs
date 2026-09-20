using UnityEngine;

public class NpcData : MonoBehaviour
{
	[SerializeField] private DressUpCharacter dressUpCharacter;
	private TimePeriod _goal;
	public TimePeriod GetGoal() => _goal;

	public void RandomizeDressUpCharacter()
	{
		int startingItemCount = Random.Range(1, 5);

		while (startingItemCount > 0)
		{
			TimeItem item = ItemDatabase.Instance.GetRandomItem();

			if (dressUpCharacter.IsItemBusy(item))
			{
				continue;
			}

			dressUpCharacter.WearItem(item);
			startingItemCount--;
		}
	}

	public void RandomizeGoalTimePeriod()
	{
		TimePeriod[] periods = (TimePeriod[])System.Enum.GetValues(typeof(TimePeriod));
		_goal = periods[Random.Range(0, periods.Length)];
	}

}
