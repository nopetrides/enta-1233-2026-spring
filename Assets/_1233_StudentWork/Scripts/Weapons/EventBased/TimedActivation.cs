using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A timer component that triggers a UnityEvent after a set duration.
/// </summary>
public class TimedActivation : MonoBehaviour
{
	[SerializeField] private float _duration = 2f;
	[SerializeField] private bool _autoReset;
	[SerializeField] private bool _playOnEnable = true;

	[SerializeField] private UnityEvent _onElapsed;
	private bool _isRunning;

	private float _timeRemaining;

	private void Update()
	{
		if (!_isRunning)
			return;

		_timeRemaining -= Time.deltaTime;

		if (_timeRemaining > 0f)
			return;

		_onElapsed?.Invoke();

		if (_autoReset)
			_timeRemaining = _duration;
		else
			_isRunning = false;
	}

	private void OnEnable()
	{
		if (_playOnEnable)
			StartTimer();
	}

	/// <summary>
	/// Starts the timer from the full duration.
	/// </summary>
	public void StartTimer()
	{
		_timeRemaining = _duration;
		_isRunning = true;
	}

	/// <summary>
	/// Stops the timer from counting down.
	/// </summary>
	public void StopTimer()
	{
		_isRunning = false;
	}

	/// <summary>
	/// Resets the remaining time to the full duration without changing the running state.
	/// </summary>
	public void ResetTimer()
	{
		_timeRemaining = _duration;
	}
}
