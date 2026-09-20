using System.Collections;
using UnityEngine;
using UnityEngine.tvOS;

public class DressUpCharacter : MonoBehaviour
{
	public TimeHat NpcHat { get {  return _npcHat; } }
	private TimeHat _npcHat;
	public TimeCloth NpcCloth {  get { return _npcCloth; } }
	private TimeCloth _npcCloth;
	private GameObject _hatObject;
	private StorageManager _storage;

	private AccessoryInteractable[] _accessories;

	enum Gender
	{
		Male,
		Female
	}
	[SerializeField] private Gender _gender;
	[SerializeField] private Transform _hatParent;
	[SerializeField] private SkinnedMeshRenderer _clothRenderer;

	private void Start()
	{
		_npcHat = null;
		_hatObject = null;
		_clothRenderer.gameObject.SetActive(false);

		_storage = StorageManager.Instance;
		if(_storage == null) _storage = FindFirstObjectByType<StorageManager>();

		_accessories = GetComponentsInChildren<AccessoryInteractable>(true);

		/*foreach (AccessoryInteractable accessory in _accessories)
			accessory.gameObject.SetActive(false);
		*/
	}

	public bool IsItemBusy(TimeItem item)
	{
		if (item is TimeHat) return _npcHat != null;
		if (item is TimeCloth) return _npcCloth != null;

		return false;
	}

	public void WearItem(TimeItem item)
	{
		if(item is TimeHat)
		{

				_npcHat = (TimeHat)item;
			
			if(_hatObject != null)
				GameObject.Destroy(_hatObject);

			_hatObject = GameObject.Instantiate(_npcHat.OnCharacterPrefab, _hatParent);
			_hatObject.transform.localPosition = Vector3.zero;
			_hatObject.transform.localRotation = Quaternion.identity;

			HatInteractable interact = _hatObject.GetComponent<HatInteractable>();

			if(interact == null)
				Debug.LogError("No Hat Interactable found on the hat you're trying to wear, you won't be able to remove the hat", this.gameObject);

			interact.AssociatedCharacter = this;

		} else if (item is TimeCloth)
		{

			_npcCloth = (TimeCloth) item;
			_clothRenderer.gameObject.SetActive(true);
			_clothRenderer.enabled = true;

			_clothRenderer.sharedMaterial = _gender == Gender.Male ? _npcCloth.ClothMaterial_M : _npcCloth.ClothMaterial_F;
		} else if (item is TimeAccessory)
		{
			TimeAccessory crntItem = (TimeAccessory) item;

			Debug.Log("Wearing an accessory");
			foreach(AccessoryInteractable accessory in _accessories)
			{
				if (crntItem.AccessoryName.Equals(accessory.AssociatedItem.AccessoryName))
					accessory.gameObject.SetActive(true);
			}
		}
	}

	public void RemoveHat()
	{
		bool removed = true;
		if (!_npcHat.CannotGoInStorage)
			removed = _storage.AddItem(_npcHat.InStoragePrefab);

		if (!removed) return;

		if (_hatObject != null)
			GameObject.Destroy(_hatObject);

		_npcHat = null;
		_hatObject = null;
	}

	public void RemoveCloth()
	{
		bool removed = true;
		if (!_npcCloth.CannotGoInStorage)
			removed = _storage.AddItem(_npcCloth.InStoragePrefab);
		if (!removed) return;

		_clothRenderer.gameObject.SetActive(false);
		_npcCloth = null;
	}
}