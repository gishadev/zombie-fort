using System;
using System.Collections.Generic;

namespace gishadev.tools.StateMachine
{
    public class StateMachine
    {
        private IState _state;
        public IState CurrentState => _state;

        private readonly Dictionary<IState, List<Transition>> _transitionsDict = new();
        private readonly List<Transition> _anyTransitions = new();
        private readonly List<Transition> _emptyTransitions = new();
        private List<Transition> _currentTransitions = new();

        public event Action<IState> OnStateChanged;



        public void Tick()
        {
            var transition = GetTransition();
            if (transition != null)
                SetState(transition.To);

            CurrentState?.Tick();
        }

        public void SetState(IState state)
        {
            if (state == CurrentState) return;
            CurrentState?.OnExit();
            _state = state;

            _transitionsDict.TryGetValue(CurrentState, out _currentTransitions);

            if (_currentTransitions == null)
                _currentTransitions = _emptyTransitions;

            CurrentState?.OnEnter();
            OnStateChanged?.Invoke(CurrentState);
        }

        private Transition GetTransition()
        {
            foreach (var transition in _anyTransitions)
            {
                if (transition.Condition())
                    return transition;
            }

            foreach (var transition in _currentTransitions)
            {
                if (transition.Condition())
                    return transition;
            }

            return null;
        }

        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            if (_transitionsDict.TryGetValue(from, out var transitions) == false)
            {
                transitions = new List<Transition>();
                _transitionsDict.Add(from, transitions);
            }

            transitions.Add(new Transition(to, condition));
        }

        public void AddAnyTransition(IState to, Func<bool> condition)
        {
            _anyTransitions.Add(new Transition(to, condition));
        }

        private class Transition
        {
            public IState To { get; }
            public Func<bool> Condition { get; }

            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }
        }
    }
}