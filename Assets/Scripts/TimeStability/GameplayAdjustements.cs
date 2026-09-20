using UnityEngine;

[CreateAssetMenu(fileName = "Gameplay Adjustements", menuName = "Scriptable Objects/Gameplay/Gameplay Adjustements")]
public class GameplayAdjustements : ScriptableObject
{
    public int BaseCharacterScore { get { return _baseCharacterScore; } }
    public float GoodItemMultiplier { get { return _goodItemMultiplier; } }
    public float BadItemMultiplier { get { return _badItemMultiplier; }}


    public int MaxStability { get { return _maxStability; } }
    public float DecreaseTime { get { return _decreaseTime; } }
    public int MinDecreaseValue {  get { return _minDecreaseValue; } }
    public int MaxDecreaseValue { get {return _maxDecreaseValue; } }
    public int SendSomeoneIncrement {  get { return _sendSomeoneIncrement; } }


    [Header("Score Calculation")]

    [Tooltip("Base character score before adding outfit")]
        [SerializeField] private int _baseCharacterScore = -50;
    [Tooltip("Multiplier for item score that correspond to the time period")]
        [SerializeField] private float _goodItemMultiplier = 1f;
    [Tooltip("Multiplier for the item score that don't correspond to the time period")]
        [SerializeField] private float _badItemMultiplier = 2f;

    [Space(10)]

    [Header("Time stability")]

    [Tooltip("The Maximum Amount of Stability, Stability starts at this value")]
        [SerializeField] private int _maxStability = 10000;

    [Space(1)]

    [Tooltip("The time in seconds the stability is decreased")]
        [SerializeField] private float _decreaseTime = 1f;
    [Tooltip("The minimum value of stability drecrease every drecrease time")]
        [SerializeField] private int _minDecreaseValue = 5;
    [Tooltip("The maximum value of stability decrease. Every time someone is sent, it goes from the min value to this one")]
        [SerializeField] private int _maxDecreaseValue = 25;
    [Tooltip("The amount the decreaseValue is incremented every time someone is sent")]
        [SerializeField] private int _sendSomeoneIncrement = 1;

    [Space(10)]

    [Header("Random Events")]
	[Tooltip("Percentage Threshold to start calculating random event")]
	    [SerializeField] private float _percentThresholdForRandom = 70f;
}