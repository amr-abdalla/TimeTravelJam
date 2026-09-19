using UnityEngine;

[CreateAssetMenu(fileName = "Time Hat", menuName = "Scriptable Objects/Time Hat")]
public class TimeHat : TimeItem
{
	public GameObject OnCharacterPrefab { get {  return _onCharacterPrefab; } }

	[Tooltip("Prefab of the hat that will be displayed on the character")]
		[SerializeField] private GameObject _onCharacterPrefab;
}