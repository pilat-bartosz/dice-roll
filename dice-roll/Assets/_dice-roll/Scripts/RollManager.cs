using System;
using _dice_roll.Dice;
using UnityEngine;
using UnityEngine.Assertions;

using DiceState = _dice_roll.Dice.Dice.DiceState;

namespace DiceRoll
{
    public class RollManager : MonoBehaviour
    {
        [SerializeField] private RigidbodyDice _dice;
        [SerializeField] private DiceRollUI _ui;

        private int _lastValue;
        private int _totalValue;

        private void Awake()
        {
            Assert.IsNotNull(_dice, "Missing dice reference");
            Assert.IsNotNull(_ui, "Missing ui reference");
        
            //Initialize gui values
            _ui.UpdateValues(-1, _totalValue);

            _dice.OnStateChange += OnDiceStateChange;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _dice.AutoRoll();
            }
        }

        private void OnDiceStateChange(Dice.DiceState diceState)
        {
            switch (diceState)
            {
                case DiceState.PickedUp:
                    OnDicePickup();
                    break;
                case DiceState.Thrown:
                    break;
                case DiceState.SettleDown:
                    OnDiceSettleDown();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(diceState), diceState, null);
            }
        }

        private void OnDicePickup()
        {
            _ui.UpdateValues(-1, _totalValue);
        }

        private void OnDiceSettleDown()
        {
            _lastValue = _dice.CheckValue;
            _totalValue += _lastValue;
            _ui.UpdateValues(_lastValue, _totalValue);
        
            Debug.Log($"Dice rolled: {_lastValue}");
        }
    }
}