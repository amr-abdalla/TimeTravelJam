using System.Collections.Generic;
using UnityEngine;

public class TextHelper : MonoBehaviour
{
	public static TextHelper Instance { get; private set; }

	private const string _periodPlaceholder = "<period>";
	[SerializeField] private string[] NpcDialogue;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning($"Two instances of {GetType()} detected, destroying one");
			Destroy(Instance);
		}
		Instance = this;
	}

	private static Dictionary<TimePeriod, string> textByTimePeriod = new Dictionary<TimePeriod, string>()
	{
		{ TimePeriod.Antiquity, "Antiquity" },
		{ TimePeriod.MiddleAge, "Middle Age" },
		{ TimePeriod.Nobility, "Nobility" },
		{ TimePeriod.Prehistory, "Prehistory" }
	};

	public string GetTimePeriodText(TimePeriod timePeriod) => textByTimePeriod[timePeriod];

	public string GetNPCText(NpcData npcData)
	{
		string timePeriodText = GetTimePeriodText(npcData.GetGoalTimePeriod());
		string dialogue = NpcDialogue[Random.Range(0, NpcDialogue.Length)];

		return dialogue.Replace(_periodPlaceholder, timePeriodText);
	}
}
