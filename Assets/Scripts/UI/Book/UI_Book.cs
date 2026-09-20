using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_Book : MonoBehaviour
{
	[System.Serializable]
	struct _page {
		public TMP_Text	itemName;
		public TMP_Text	itemPeriod;
		public TMP_Text	itemDescription;
		public Image	itemImage;
	}

	[SerializeField] private _page[] pages;

	private StabilityManager _stability;
	private ItemDatabase _db;

	public int CurrentIndex
	{
		get { return _currentIndex; }
		set { 
			_currentIndex = value;
			if (_currentIndex >= _db.AllItems.Length)
				_currentIndex = 0;
			if(_currentIndex < 0)
				_currentIndex = _db.AllItems.Length - 1;
			updateBook();
		}
	}
	[SerializeField] private int _currentIndex;

	private void Start()
	{
		_stability = StabilityManager.Instance;
		_db = ItemDatabase.Instance;
		_currentIndex = 0;

		updateBook();
	}

	private void updateBook()
	{
		TimeItem firstItem = _db.AllItems[_currentIndex];
		updatePage(firstItem, pages[0]);

		if (_currentIndex + 1 < _db.AllItems.Length)
		{
			TimeItem secondItem = _db.AllItems[_currentIndex+1];
			updatePage(secondItem, pages[1]);
		}
		else
			cleanPage(pages[1]);
	}

	private void updatePage(TimeItem item, _page page)
	{
		page.itemName.text = item.ItemName;
		page.itemPeriod.text = item.Period.ToString(); //TODO : change this to use the magic function
		if (_stability.HasAnomaly(item))
			page.itemPeriod.text += $" is now from <color=purple>{_stability.GetAnomaly(item)}</color>";

		page.itemDescription.text = item.DefaultDescription;

		foreach(TimeItem.ItemDescription desc in item.PeriodsDescription)
		{
			if(desc.period == _stability.GetAnomaly(item))
			{
				page.itemDescription.text = desc.description;
			}
		}
	}

	private void cleanPage(_page page)
	{
		page.itemName.text = string.Empty;
		page.itemPeriod.text = string.Empty;
		page.itemDescription.text = string.Empty;

		page.itemImage.sprite = null;
	}

	public void OpenBook()
	{
		Time.timeScale = 0f;
	}

	public void ClostBook()
	{
		Time.timeScale = 1f;
		this.gameObject.SetActive(false);
	}

	public void IncrementPage()
	{
		CurrentIndex++;
	}

	public void DecrementPage()
	{
		CurrentIndex--;
	}
}
