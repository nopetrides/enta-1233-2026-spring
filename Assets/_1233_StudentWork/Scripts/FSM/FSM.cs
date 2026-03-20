using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets._1233_StudentWork.Scripts.FSM {
    public sealed class FSM<TCharacter, TInput> {
        private readonly TCharacter _character;
        private readonly Dictionary<Type, FSM_State<TCharacter, TInput>> _states = new();
        private readonly Dictionary<Type, bool> _enabled = new();
        private readonly Dictionary<Type, List<TransitionRecord>> _transitionsFrom = new();

        private FSM_State<TCharacter, TInput> _current;

        private readonly struct TransitionRecord {
            public readonly FSM_Transition<TCharacter, TInput> Instance;
            public readonly Type ToType;
            public readonly int Priority;

            public TransitionRecord(FSM_Transition<TCharacter, TInput> instance, Type toType, int priority) {
                Instance = instance;
                ToType = toType;
                Priority = priority;
            }
        }

        public FSM(in TCharacter character, IEnumerable<FSM_State<TCharacter, TInput>> states, IEnumerable<Type> transitionTypes, FSM_State<TCharacter, TInput> initialState, in TInput initialInput) {
            if ( states == null )
                throw new ArgumentNullException(nameof(states));
            if ( transitionTypes == null )
                throw new ArgumentNullException(nameof(transitionTypes));
            if ( initialState == null )
                throw new ArgumentNullException(nameof(initialState));

            // Register states (instances)
            foreach ( var s in states ) {
                if ( s == null )
                    throw new ArgumentException("States list contained null.", nameof(states));
                var t = s.GetType();

                if ( _states.ContainsKey(t) )
                    throw new ArgumentException($"Duplicate state instance for type {t.Name}. Provide only one instance per concrete type.");

                s.FSM = this;
                _states[t] = s;
                _enabled[t] = true;
            }

            // initialState must be one of the registered instances
            var initType = initialState.GetType();
            if ( !_states.TryGetValue(initType, out var registeredInit) || !ReferenceEquals(registeredInit, initialState) )
                throw new ArgumentException("initialState must be the same instance as one of the provided states.", nameof(initialState));

            // Instantiate transitions and build adjacency
            foreach ( var tt in transitionTypes ) {
                if ( tt == null )
                    throw new ArgumentException("Transition type list contained null.", nameof(transitionTypes));
                if ( !typeof(FSM_Transition<TCharacter, TInput>).IsAssignableFrom(tt) )
                    throw new ArgumentException($"{tt.Name} does not inherit FSM_Transition<{typeof(TInput).Name}>");

                var tr = (FSM_Transition<TCharacter, TInput>)Activator.CreateInstance(tt)!;
                tr.FSM = this;

                var (fromTypes, toType, priority) = ReadTransitionMeta(tt);

                if ( !_states.ContainsKey(toType) )
                    throw new ArgumentException($"Transition {tt.Name} targets {toType.Name}, but that state was not provided.");

                // For each declared "From", attach this transition to *all* registered states
                // that are the declared type OR derive from it.
                var attachedTo = new HashSet<Type>();

                foreach ( var declaredFrom in fromTypes ) {
                    if ( declaredFrom == null )
                        throw new ArgumentException($"Transition {tt.Name}.From contains null.");

                    if ( !typeof(FSM_State<TCharacter, TInput>).IsAssignableFrom(declaredFrom) )
                        throw new ArgumentException(
                            $"Transition {tt.Name} declares From {declaredFrom.Name}, but it is not a FSM_State<{typeof(TCharacter).Name}, {typeof(TInput).Name}>.");

                    // Find all registered state types compatible with declaredFrom
                    foreach ( var registeredStateType in _states.Keys ) {
                        // declaredFrom.IsAssignableFrom(registeredStateType) means:
                        // registeredStateType == declaredFrom OR registeredStateType derives from declaredFrom
                        if ( !declaredFrom.IsAssignableFrom(registeredStateType) )
                            continue;

                        if ( !attachedTo.Add(registeredStateType) )
                            continue; // avoid duplicates if multiple declaredFrom types overlap

                        if ( !_transitionsFrom.TryGetValue(registeredStateType, out var list) ) {
                            list = new List<TransitionRecord>();
                            _transitionsFrom[registeredStateType] = list;
                        }

                        list.Add(new TransitionRecord(tr, toType, priority));
                    }
                }
            }

            // Sort by priority (high first)
            foreach ( var kvp in _transitionsFrom )
                kvp.Value.Sort((a, b) => b.Priority.CompareTo(a.Priority));

            // Init all states
            foreach ( var s in _states.Values )
                s.Init();

            // Enter initial state
            _character = character;
            _current = initialState;
            _current.ResetElapsedTime();
            _current.OnEnter(0, _character, in initialInput);
        }

        // ---------- API ----------

        public TState? GetStateOfClass<TState>() where TState : FSM_State<TCharacter, TInput>
            => _states.TryGetValue(typeof(TState), out var s) ? (TState)s : null;

        public bool SetStateEnabled<TState>(bool enabled) where TState : FSM_State<TCharacter, TInput> {
            var t = typeof(TState);
            if ( !_states.ContainsKey(t) )
                return false;
            _enabled[t] = enabled;
            return true;
        }

        public FSM_State<TCharacter, TInput> GetState() => _current;

        /// <summary>
        /// Instantly forces the FSM into the given state class, if present.
        /// If respectEnabled is true, will fail when the target state is disabled.
        /// Returns true if successful.
        /// </summary>
        public bool SetState<TState>(float deltaTime, in TInput input, bool respectEnabled)
            where TState : FSM_State<TCharacter, TInput>
            => SetState(typeof(TState), deltaTime, in input, respectEnabled);

        public void Step(float deltaTime, in TInput input) {
            var curType = _current.GetType();

            // Transitions (respect enabled by definition)
            if ( _transitionsFrom.TryGetValue(curType, out var list) ) {
                for ( int i = 0; i < list.Count; i++ ) {
                    var rec = list[i];

                    // Prevent self-transition
                    if ( rec.ToType == curType )
                        continue;

                    // Respect enabled/disabled target states
                    if ( _enabled.TryGetValue(rec.ToType, out var isEnabled) && !isEnabled )
                        continue;

                    if ( rec.Instance.Test(deltaTime, in _character, in input) ) {
                        // transitions always respect enabled status
                        SetState(rec.ToType, deltaTime, in input, respectEnabled: true);
                        break; // one transition per step
                    }
                }
            }

            // Time in state increases regardless of transitions taken this frame (after any transition)
            _current.AddElapsedTime(deltaTime);

            // State logic
            _current.Step(deltaTime, in _character, in input);
        }

        // ---------- Optional helpers ----------

        public FSM_State<TCharacter, TInput>? GetStateOfClass(Type exactStateType)
            => exactStateType != null && _states.TryGetValue(exactStateType, out var s) ? s : null;

        // ---------- Internals ----------

        private bool SetState(Type exactStateType, float deltaTime, in TInput input, bool respectEnabled) {
            if ( !_states.TryGetValue(exactStateType, out var next) )
                return false;

            if ( ReferenceEquals(_current, next) )
                return false; // no self-transition / redundant set

            if ( respectEnabled &&
                _enabled.TryGetValue(exactStateType, out var isEnabled) &&
                !isEnabled )
                return false;

            _current.OnLeave(deltaTime, in _character, in input);

            _current = next;
            _current.ResetElapsedTime();
            _current.OnEnter(deltaTime, in _character, in input);

            return true;
        }

        private static (Type[] from, Type to, int priority) ReadTransitionMeta(Type transitionType) {
            var flags = BindingFlags.Public | BindingFlags.Static;

            var fromProp = transitionType.GetProperty("From", flags);
            var toProp = transitionType.GetProperty("To", flags);
            var prProp = transitionType.GetProperty("Priority", flags);

            if ( fromProp == null || toProp == null )
                throw new ArgumentException($"Transition {transitionType.Name} must declare public static: Type[] From and Type To.");

            var fromVal = fromProp.GetValue(null) as Type[];
            var toVal = toProp.GetValue(null) as Type;

            if ( fromVal == null )
                throw new ArgumentException($"{transitionType.Name}.From must be a Type[].");
            if ( toVal == null )
                throw new ArgumentException($"{transitionType.Name}.To must be a Type.");

            int priority = 0;
            if ( prProp != null ) {
                var pObj = prProp.GetValue(null);
                if ( pObj is int p )
                    priority = p;
                else
                    throw new ArgumentException($"{transitionType.Name}.Priority must be an int if provided.");
            }

            if ( fromVal.Any(t => t == null) )
                throw new ArgumentException($"{transitionType.Name}.From contains null.");

            return (fromVal, toVal, priority);
        }
    }
}
