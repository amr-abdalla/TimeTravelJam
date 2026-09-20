using UnityEngine;

public class StorageInteractable : Interactable
{
	[SerializeField] private TimeItem _associatedItem;

	[HideInInspector] public int CurrentStorageIndex;

	private StorageManager _storage;

	private void Start()
	{
		_storage = StorageManager.Instance;
		if (_storage == null) _storage = FindFirstObjectByType<StorageManager>();
	}

	public override void Interaction()
	{
		base.Interaction();
		_storage.ItemWearCallback(CurrentStorageIndex, _associatedItem);
	}
}