using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilityManager : MonoBehaviour
{
	public static StabilityManager Instance { get; private set; }

	[SerializeField] private GameplayAdjustements _adjustements;

	private Dictionary<TimeItem, TimePeriod> _timeAnomalies;

	public int CurrentStability { get { return _currentStability; } }
	[SerializeField] private int _currentStability;

	private int _currentDecrease;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning("Two instances of StabilityManager detected, destroying one");
			Destroy(Instance);
		}
		Instance = this;
	}

	private void Start()
	{
		_timeAnomalies = new Dictionary<TimeItem, TimePeriod>();

		if (_adjustements == null) Debug.LogError("Gameplay adjustements screen is Missing on Stabiliy Manager");

		_currentStability = _adjustements.MaxStability;
		_currentDecrease = _adjustements.MinDecreaseValue;

		StartCoroutine(stabilityCoroutine());
		StartCoroutine(testSomeoneThroughTime());
	}

	public bool HasAnomaly(TimeItem item) => _timeAnomalies.ContainsKey(item);

	public TimePeriod GetAnomaly(TimeItem timeItem)
	{
		if(_timeAnomalies.ContainsKey(timeItem))
			return _timeAnomalies[timeItem];

		return timeItem.Period;
	}

	public void SendSomeoneThroughTime(DressUpCharacter character, TimePeriod period) //call this when warping someone
	{
		_currentDecrease += _adjustements.SendSomeoneIncrement;

		if (_currentDecrease > _adjustements.MaxDecreaseValue)
			_currentDecrease = _adjustements.MaxDecreaseValue;

		int score = character.CalculateOutfitScore(period, _adjustements);

		_currentStability += score;

		CheckForAnomaly();
	}

	private IEnumerator stabilityCoroutine()
	{
		while (gameObject.activeSelf)
		{
			yield return new WaitForSeconds(_adjustements.DecreaseTime);

			_currentStability -= _currentDecrease;
		}
	}

	private IEnumerator testSomeoneThroughTime()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(1, 4));
			SendSomeoneThroughTime(FindFirstObjectByType<DressUpCharacter>(), (TimePeriod)Random.Range(0, 4));
		}
	}

	public void CheckForAnomaly()
	{
		float thresholdValue = ((_adjustements.ThresholdForRandom * _adjustements.MaxStability) / 100);

		if (_currentStability > thresholdValue)
			return;

		//Debug.Log("Check");

		float dice = Random.Range(0, thresholdValue);

		if(dice > _currentStability)
		{
			Debug.Log("ANOMALY");
			//Get Random Item
			//Add Anomalu
		}
	}
}