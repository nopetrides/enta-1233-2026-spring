using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	// Avoid using GetComponent
	[SerializeField] private CharacterController _characterController;
	[SerializeField] private float _speed = 5f;
	[SerializeField] private float _smoothTime = 0.05f;
	[SerializeField] private float _gravityMultiplier = 3f;
	[SerializeField] private float _jumpStrength = 5f;
	[SerializeField] private int _maxNumberOfJumps = 3;
	
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
	[SerializeField]
	private bool _isGrounded;
	[SerializeField]
	private int _numberOfJumps;
	
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
		if (!context.started) return;
		if (!_isGrounded && _numberOfJumps >= _maxNumberOfJumps) return;
		_numberOfJumps++;
		_gravityVelocity = _jumpStrength;
		_isGrounded = false;
	}

	/// <summary>
	/// Called each frame
	/// </summary>
	public void Update()
	{
		Gravity();
		Rotate();
		Move();
		AnimationParameters();
	}

	private void Rotate()
	{
		// Prevents character from rotating when no input
		if (_moveInput.sqrMagnitude < 0.01f) return;
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
		if (_isGrounded) 
			_gravityVelocity = -1f;
		else
			_gravityVelocity += _gravity * _gravityMultiplier * Time.deltaTime;
		
		_direction.y = _gravityVelocity;
	}

	private void GroundCheck()
	{
		// Specific ground check so we only check once per frame
		_isGrounded = _characterController.isGrounded;
		if (_isGrounded) _numberOfJumps = 0;
	}

	/// <summary>
	/// Not using the coroutine method from the example.
	/// I prefer to know exactly when the grounded check is.
	/// </summary>
	/// <returns></returns>
	private System.Collections.IEnumerator WaitForLanding()
	{
		yield return new WaitUntil(() => !_isGrounded);
		yield return new WaitUntil(() => _characterController.isGrounded);
		
		_numberOfJumps = 0;
	}
	
	[SerializeField] 
	private Animator _animator;
	
	private static readonly int Speed = 
		Animator.StringToHash("Speed");
	
	private void AnimationParameters()
	{
		_animator.SetFloat(
			Speed, _moveInput.sqrMagnitude);
	}
	
}
