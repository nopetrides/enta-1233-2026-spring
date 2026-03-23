using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Reusable health bar visual for any <see cref="Health" /> component.
///     Intended for enemy world-space UI, but also works for other actors.
/// </summary>
public class HealthBarUI : MonoBehaviour
{
	[SerializeField] private Transform _canvasTransform;
	[SerializeField] private Health _health;
	[SerializeField] private Image _fillImage;

	private Camera _cameraReference;
	private Camera CameraToRotateWith => _cameraReference ??= Camera.main;
	
	private void OnEnable()
	{
		if (_health == null)
		{
			Debug.LogError($"HealthBarUI: No Health assigned on {name}.");
			Refresh(null);
			return;
		}

		_health.OnHealthChanged += Refresh;
		_health.OnDied += HandleDied;
		Refresh(_health);
	}

	private void OnDisable()
	{
		if (_health == null) return;

		_health.OnHealthChanged -= Refresh;
		_health.OnDied -= HandleDied;
	}

	private void HandleDied()
	{
		_fillImage.fillAmount = 0;
	}

	private void Refresh(Health health)
	{
		_fillImage.fillAmount = health != null ? health.NormalizedHealth : 0f;
	}

	private void Update()
	{
		if (CameraToRotateWith == null)
			return;
		_canvasTransform.rotation = CameraToRotateWith.transform.rotation;
	}
}
