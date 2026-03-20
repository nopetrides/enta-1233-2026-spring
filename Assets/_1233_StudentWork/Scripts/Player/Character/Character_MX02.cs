using Assets._1233_StudentWork.Scripts.FSM;
using Assets._1233_StudentWork.Scripts.PlayerCharacter.States;
using Assets._1233_StudentWork.Scripts.PlayerCharacter.Transitions;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Character_MX02 : Character {

	[SerializeField] private Animator _animator;
	[SerializeField] private Health _health;

	private static Dictionary<Type, int> STATE_POSE_ID_MAP = new() {
		{ typeof(Idling), 0 },
		{ typeof(Locomoting), 1 },
		{ typeof(Jumping), 2 },
		{ typeof(Freefall), 3 },
		{ typeof(Landed), 4 },
		{ typeof(Dead), 10 },
	};

	private static List<Type> DISALLOWED_ATTACK_STATES = new() {
		typeof(Jumping),
		typeof(Freefall),
		typeof(Dead),
	};

	public FSM<Character_MX02, PlayerCharacterInput> StateMachine { get; private set; }
	public Vector3 velocity = Vector3.zero;
	public float walkSpeed = 1f;
	public float jumpPower = 1f;

	private bool _hasAttacked = false;
	private bool _canAttack = true;

	void Awake() {}

	void Start() {
		Player player = PlayerService.Instance.GetPlayerFromCharacter(this);

		List<MX02StateBase> states = new() {
			new Idling(),
			new Locomoting(),
			new Jumping(),
			new Landed(),
			new Freefall(),
			new Dead(),
		};

		List<Type> transitions = new() {
			typeof(Fall),
			typeof(Jump),
			typeof(Land),
			typeof(LocomotionStart),
			typeof(LocomotionStop),
		};

		StateMachine = new(this, states, transitions, states[0], player.CharacterInputs);

		_health.OnDamaged += (HealthModifyInfo _) => {
			_animator.SetTrigger("Hit");
		};

		_health.OnDied += () => {
			Player player = PlayerService.Instance.GetPlayerFromCharacter(this);
			StateMachine.SetState<Dead>(0f, player.CharacterInputs, false);
			_animator.SetTrigger("Died");
		};
	}

	public void SetCanAttack(bool enabled) {
		_canAttack = enabled;
	}

	void Update() {
		float dt = Time.deltaTime;
		Player player = PlayerService.Instance.GetPlayerFromCharacter(this);
		PlayerCharacterInput input = player.CharacterInputs;
		StateMachine.Step(dt, input);
		Controller.Move(velocity * dt);

		FSM_State<Character_MX02, PlayerCharacterInput> currentState = StateMachine.GetState();
		if ( STATE_POSE_ID_MAP.TryGetValue(currentState.GetType(), out int poseId))
			_animator.SetInteger("PoseId", poseId);

		if ( _canAttack && input.Attack && !_hasAttacked && !DISALLOWED_ATTACK_STATES.Contains(StateMachine.GetState().GetType()) ) {
			_animator.SetTrigger("Attack");
		}
		_hasAttacked = input.Attack;
	}

}
