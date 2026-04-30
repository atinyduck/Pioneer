using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Pioneer.Puzzle
{
    public enum LogicGateType
    {
        AND,
        OR,
        NOT // Note: NOT gate typically uses only one input (the first in the list)
    }

    /// <summary>
    /// Evaluates the state of multiple FloorButtons based on a Logic Gate type (AND, OR, NOT).
    /// Fires UnityEvents when the evaluated state changes.
    /// </summary>
    public class LogicGate : MonoBehaviour
    {
        [Header("Gate Settings")]
        [Tooltip("The type of logic gate (AND, OR, NOT)")]
        public LogicGateType gateType = LogicGateType.AND;

        [Tooltip("The FloorButtons that act as inputs to this logic gate")]
        public List<FloorButton> inputs = new List<FloorButton>();

        [Header("Output Events")]
        [Tooltip("Fired when the logic gate condition is MET (True)")]
        public UnityEvent OnGateActivated;

        [Tooltip("Fired when the logic gate condition is NO LONGER MET (False)")]
        public UnityEvent OnGateDeactivated;

        [Header("Debug state")]
        [SerializeField] private bool currentState = false;

        private void OnEnable()
        {
            // Subscribe to each floor button's state change event
            foreach (var button in inputs)
            {
                if (button != null)
                {
                    button.OnButtonStateChanged += EvaluateGate;
                }
            }
        }

        private void OnDisable()
        {
            // Clean up subscriptions
            foreach (var button in inputs)
            {
                if (button != null)
                {
                    button.OnButtonStateChanged -= EvaluateGate;
                }
            }
        }

        private void Start()
        {
            // Evaluate initial state on startup
            EvaluateGate(false);
        }

        /// <summary>
        /// Evaluates the inputs based on the LogicGateType and triggers events if the state changes.
        /// </summary>
        private void EvaluateGate(bool dummyState)
        {
            if (inputs.Count == 0) return;

            bool nextState = false;

            switch (gateType)
            {
                case LogicGateType.AND:
                    nextState = true;
                    foreach (var input in inputs)
                    {
                        if (!input.IsPressed)
                        {
                            nextState = false;
                            break;
                        }
                    }
                    break;
                case LogicGateType.OR:
                    nextState = false;
                    foreach (var input in inputs)
                    {
                        if (input.IsPressed)
                        {
                            nextState = true;
                            break;
                        }
                    }
                    break;
                case LogicGateType.NOT:
                    // NOT usually takes only one input
                    if (inputs[0] != null)
                    {
                        nextState = !inputs[0].IsPressed;
                    }
                    break;
            }

            // Only fire events if the state has changed
            if (nextState != currentState)
            {
                currentState = nextState;
                if (currentState)
                {
                    OnGateActivated?.Invoke();
                }
                else
                {
                    OnGateDeactivated?.Invoke();
                }
            }
        }
    }
}