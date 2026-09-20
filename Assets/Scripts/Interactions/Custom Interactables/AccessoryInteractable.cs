using UnityEngine;

public class AccessoryInteractable : Interactable
{
	public TimeAccessory AssociatedItem { get { return _associatedItem; } }
	[SerializeField] private TimeAccessory _associatedItem;

	private StorageManager _storage;

	private void Start()
	{
		_storage = StorageManager.Instance;
		/*
		if(_associatedItem.CannotGoInStorage)
			_outline.OutlineColor = Color.red;
		*/
	}

	public override void Interaction()
	{
		base.Interaction();
		bool added = true;

		if (!_associatedItem.CannotGoInStorage)
			added = _storage.AddItem(_associatedItem.InStoragePrefab);

		if (!added)
			return;

		this.gameObject.SetActive(false);
	}
}
