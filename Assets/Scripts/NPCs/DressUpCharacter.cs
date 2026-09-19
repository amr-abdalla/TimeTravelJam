using UnityEngine;

public class DressUpCharacter : MonoBehaviour
{
	private TimeHat _npcHat;
	private TimeCloth _npcCloth;
	private GameObject _hatObject;

	[SerializeField] private Transform _hatParent;
	[SerializeField] private SkinnedMeshRenderer _clothRenderer;

	private void Start()
	{
		_npcHat = null;
		_hatObject = null;
		_clothRenderer.gameObject.SetActive(false);
	}

	public void WearItem(TimeItem item)
	{
		if(item is TimeHat)
		{
			_npcHat = (TimeHat) item;
			
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
			_clothRenderer.sharedMaterial = _npcCloth.ClothMaterial;
		}
	}

	public void RemoveHat()
	{
		if (_hatObject != null)
			GameObject.Destroy(_hatObject);

		_npcHat = null;
		_hatObject = null;
	}

	public void RemoveCloth()
	{
		_clothRenderer.gameObject.SetActive(false);
		_npcCloth = null;
	}
}