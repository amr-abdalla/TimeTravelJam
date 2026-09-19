using UnityEngine;

[CreateAssetMenu(fileName = "Time Item", menuName = "Scriptable Objects/Time Item")]
public class TimeItem : ScriptableObject
{
    public string ItemName { get { return _itemName; } }
    public ItemType Type { get { return _type; } }
    public TimePeriod Period { get { return _period; } }
    public GameObject OnCharacterPrefab {  get { return _onCharacterPrefab; } }
    public GameObject InStoragePregab   { get { return _inStoragePrefab; } }

    [Tooltip("Display name of the item if needed")]
        [SerializeField] private string _itemName; //if we need something different than ScriptableObject.name
    public enum ItemType
    {
        Wearable = 0,
        Accessory = 1
    }
    [Tooltip("Type of the object, Wearable automatically goes on the body, accessories must be placed on all prefabs first")]
        [SerializeField] private ItemType _type;

    [Tooltip("The time period the object belongs to")]
        [SerializeField] private TimePeriod _period;

    [Tooltip("The value of the item used to calculate score (not used for now)")]
        [SerializeField] private int _itemValue;

    [Space(5)]

    [Tooltip("Prefab of the object that will be displayed on the characer (only for wearable item)")]
        [SerializeField] private GameObject _onCharacterPrefab;
    [Tooltip("Prefab of the object when it's stored")]
        [SerializeField] private GameObject _inStoragePrefab;

	private void OnValidate()
	{
        if (_onCharacterPrefab != null && _type == ItemType.Accessory)
            Debug.LogWarning("Character prefab is not needed for accessories, they need to be placed directly on prefabs");
	}
}