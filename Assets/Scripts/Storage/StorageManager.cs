using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
	public static StorageManager Instance { get; private set; }

	[SerializeField] private TimeItem[] _startingItems;

	private Transform[] _cells;
	private Dictionary<Transform, StorageInteractable> _associations;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning("Two instances of StorageManager detected, destroying one");
			Destroy(Instance);
		}
		Instance = this;

		_associations = new Dictionary<Transform, StorageInteractable>();
	}

	private void Start()
	{
		_cells = new Transform[this.transform.childCount];

		for (int i = 0; i < _cells.Length; i++)
			_cells[i] = transform.GetChild(i);

		foreach (TimeItem item in _startingItems)
			AddItem(item.InStoragePrefab);
	}

	public bool AddItem(GameObject item)
	{
		int index = getNextAvailableCell();

		if(index < 0)
			return false;

		GameObject instantiatedItem = GameObject.Instantiate(item, _cells[index]);
		instantiatedItem.transform.localPosition = Vector3.zero;
		instantiatedItem.transform.localRotation = Quaternion.identity;

		StorageInteractable interactable = instantiatedItem.GetComponent<StorageInteractable>();

		if (interactable == null)
			Debug.LogError("StorageInteractable is null on instantiated item, it is needed to equip the item on the character", instantiatedItem);

		interactable.CurrentStorageIndex = index;

		_associations.Add(_cells[index], interactable);

		return true;
	}

	public void SwitchItem(GameObject item, int index)
	{
		RemoveItem(index);
		AddItem(item);
	}

	public bool RemoveItem(int index)
	{
		if (index >= _cells.Length)
		{
			Debug.LogError($"Wrong index : {index} while trying to remove item from storage", this.gameObject);
			return false;
		}

		GameObject.Destroy(_cells[index].GetChild(0).gameObject);

		_associations.Remove(_cells[index]);

		return true;
	}

	public void ItemWearCallback(int index, TimeItem item)
	{
		DressUpCharacter currentCharacter = NpcPositionManager.Instance.GetSelectedNPC().GetComponent<DressUpCharacter>();

		TimeItem crntCharacterItem = null;

		if (item is TimeHat)
		{
			crntCharacterItem = (TimeItem)currentCharacter.NpcHat;
		} else if (item is TimeCloth)
		{
			crntCharacterItem = (TimeItem)currentCharacter.NpcCloth;
		}

		currentCharacter.WearItem(item);
		RemoveItem(index);
		if(crntCharacterItem != null)
			AddItem(crntCharacterItem.InStoragePrefab);
	}

	private int getNextAvailableCell()
	{
		for (int i = 0; i < _cells.Length; i++)
			if (!_associations.ContainsKey(_cells[i])) return i;

		return -1;
	}
}
