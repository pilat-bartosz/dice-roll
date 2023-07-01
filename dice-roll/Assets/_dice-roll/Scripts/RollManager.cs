using System;
using _dice_roll.Die;
using UnityEngine;
using UnityEngine.Assertions;

namespace _dice_roll
{
    /// <summary>
    /// Dummy class to manage everything
    /// </summary>
    public class RollManager : MonoBehaviour
    {
        [SerializeField] private RigidbodyDie _die;
        [SerializeField] private DiceRollUI _ui;

        private int _lastValue;
        private int _totalValue;
        
        /// <summary>
        /// Plane on which the player's hand will move when grabbing a dice.
        /// </summary>
        private Plane _handPlane;

        private void Awake()
        {
            Assert.IsNotNull(_die, "Missing dice reference");
            Assert.IsNotNull(_ui, "Missing ui reference");
        
            //Initialize gui values
            _ui.UpdateValues(-1, _totalValue);

            _die.OnStateChange += OnDiceStateChange;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _die.AutoRoll();
            }
        }

        private void OnDiceStateChange(Die.Die.DieState dieState)
        {
            switch (dieState)
            {
                case Die.Die.DieState.PickedUp:
                    OnDicePickup();
                    break;
                case Die.Die.DieState.Thrown:
                    break;
                case Die.Die.DieState.SettledDown:
                    OnDiceSettleDown();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dieState), dieState, null);
            }
        }

        private void OnDicePickup()
        {
            _ui.UpdateValues(-1, _totalValue);
        }

        private void OnDiceSettleDown()
        {
            _lastValue = _die.CheckValue;
            _totalValue += _lastValue;
            _ui.UpdateValues(_lastValue, _totalValue);
        
            Debug.Log($"Dice rolled: {_lastValue}");
        }
    }
}