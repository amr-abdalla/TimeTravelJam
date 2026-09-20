using UnityEngine;
using UnityEngine.UI;

public class UI_StabilityBar : MonoBehaviour
{
	public float Value
	{
		get { return _value; }
		set { 
			_value = value;
			_lerpTimer = 0f;
		}
	}
	private float	_value;
	public float	MaxValue;

	[SerializeField] private float _chipSpeed = 2f;
	[SerializeField] private Image _frontBar;
	[SerializeField] private Image _backBar;
	[SerializeField] private Color _fullColor;
	[SerializeField] private Color _emptyColor;
	private float _lerpTimer;

	public void InitializeBar(float maxStability, float currentStability)
	{
		MaxValue = maxStability;
		Value = currentStability;
	}

	private void Update()
	{
		_value = Mathf.Clamp(_value, 0, MaxValue); 
		UpdateBarUI();
	}

	public void UpdateBarUI()
	{
		float fillF = _frontBar.fillAmount;
		float fillB = _backBar.fillAmount;

		float vFraction = _value / MaxValue;

		if(fillB > vFraction)
		{
			_frontBar.fillAmount = vFraction;
			_frontBar.color = Color.Lerp(_emptyColor, _fullColor, vFraction);
			_backBar.color = Color.red;
			_lerpTimer += Time.deltaTime;
			float percentComplete = _lerpTimer / _chipSpeed;
			percentComplete = percentComplete * percentComplete;
			_backBar.fillAmount = Mathf.Lerp(fillB, vFraction, percentComplete);
		} if(fillF < vFraction)
		{
			_backBar.fillAmount = vFraction;
			_frontBar.color = Color.Lerp(_emptyColor, _fullColor, vFraction);
			_backBar.color = Color.green;
			_lerpTimer += Time.deltaTime;
			float percentComplete = _lerpTimer / _chipSpeed;
			percentComplete = percentComplete * percentComplete;
			_frontBar.fillAmount = Mathf.Lerp(fillF, _backBar.fillAmount, percentComplete);
		}
	}
}
