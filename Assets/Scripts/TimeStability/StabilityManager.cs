using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StabilityManager : MonoBehaviour
{
	public static StabilityManager Instance { get; private set; }

	public GameplayAdjustements Adujstements {  get {  return _adjustements; } }
	[SerializeField] private GameplayAdjustements _adjustements;
	[SerializeField] private UnityEvent _gameOverEvent;

	private Dictionary<TimeItem, TimePeriod> _timeAnomalies;

	public int CurrentStability { 
		get { return _currentStability; } 
		set
		{
			_currentStability = value;
			_stabilityBar.Value = (float) (_currentStability);
			if (_currentStability <= 0)
			{
				Debug.Log("Game Over");
				_gameOverEvent.Invoke();
			}
		}
	}
	[SerializeField] private int _currentStability;

	[SerializeField] private UI_StabilityBar _stabilityBar;

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

		_stabilityBar.InitializeBar((float) _adjustements.MaxStability, (float)_currentStability);

		StartCoroutine(stabilityCoroutine());
		//StartCoroutine(testSomeoneThroughTime());
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

		CurrentStability += score;

		CheckForAnomaly();
	}

	private IEnumerator stabilityCoroutine()
	{
		while (gameObject.activeSelf)
		{
			yield return new WaitForSeconds(_adjustements.DecreaseTime);

			CurrentStability -= _currentDecrease;
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

	public void AddAnomaly(TimeItem item, TimePeriod period) => _timeAnomalies.Add(item, period);

	public void CheckForAnomaly()
	{
		float thresholdValue = ((_adjustements.ThresholdForRandom * _adjustements.MaxStability) / 100f);

		if (CurrentStability > thresholdValue)
			return;

		//Debug.Log("Check");

		float dice = Random.Range(0, thresholdValue);

		if(dice > CurrentStability)
		{
			Debug.Log("ANOMALY");

			//Get Random Item

			TimeItem randomItem = null;
			TimePeriod randomPeriod = randomItem.Period;
			do
				randomPeriod = (TimePeriod)Random.Range(0, 4);
			while (randomPeriod != randomItem.Period);

			AddAnomaly(randomItem, randomPeriod);
		}
	}
}