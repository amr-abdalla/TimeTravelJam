using UnityEngine;

public class TimeItem : ScriptableObject
{
    public string ItemName { get { return _itemName; } }
    public TimePeriod Period { get { return _period; } }
    public GameObject InStoragePregab { get { return _inStoragePrefab; } }

    [Tooltip("Display name of the item if needed")]
    [SerializeField] protected string _itemName; //if we need something different than ScriptableObject.name

    [Tooltip("The time period the object belongs to")]
    [SerializeField] protected TimePeriod _period;

    [Tooltip("The value of the item used to calculate score (not used for now)")]
    [SerializeField] protected int _itemValue;

    [Space(5)]

    [Tooltip("Prefab of the object when it's stored")]
    [SerializeField] protected GameObject _inStoragePrefab;

    public string DefaultDescription { get { return _defaultDescription; } }
    [SerializeField][TextArea(2, 2)] private string _defaultDescription = "Use this item on a traveler going on the right time period";

    [System.Serializable]
    public struct ItemDescription
    {
        public TimePeriod period;
        [TextArea(5, 5)]
        public string description;
    }

    public ItemDescription[] PeriodsDescription { get { return _periodsDescriptions; } }
    [SerializeField] private ItemDescription[] _periodsDescriptions;
}