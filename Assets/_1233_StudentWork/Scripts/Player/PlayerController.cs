using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private CharacterController _characterController;
	[SerializeField] private float _speed = 5f;
	[SerializeField] private float _smoothTime = 0.05f;
	[SerializeField] private float _gravityMultiplier = 3f;
	
	
	[Header("Debug Serialization")]
	[SerializeField] // debug
	private Vector2 _moveInput;
	[SerializeField] // debug
	private bool _jump;
	[SerializeField] // debug
	private Vector3 _direction;
	[SerializeField]
	private float _currentVelocity;
	[SerializeField]
	private float _gravity = -9.81f;
	[SerializeField]
	private float _gravityVelocity;
	
	/// <summary>
	/// Axis (WASD or left stick) -1 to +1 for each axis
	/// </summary>
	/// <param name="context"></param>
	public void OnMove(InputAction.CallbackContext context)
	{
		_moveInput = context.ReadValue<Vector2>();
		_direction = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		_jump = context.ReadValueAsButton();
	}

	/// <summary>
	/// Called each frame
	/// </summary>
	public void Update()
	{
		Gravity();
		if (_moveInput.sqrMagnitude < 0.01f) return;
		Rotate();
		Move();
	}

	private void Rotate()
	{
		var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
		var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, _smoothTime);
		transform.rotation = Quaternion.Euler(0f, angle, 0f);
	}

	private void Move()
	{
		_characterController.Move(_direction * (_speed * Time.deltaTime));
	}

	private void Gravity()
	{
		if (_characterController.isGrounded) 
			_gravityVelocity = -1f;
		else
			_gravityVelocity += _gravity * _gravityMultiplier * Time.deltaTime;
		
		_direction.y = _gravityVelocity;
	}
}
