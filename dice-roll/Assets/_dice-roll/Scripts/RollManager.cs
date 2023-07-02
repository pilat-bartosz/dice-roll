using System;
using _dice_roll.Die;
using _dice_roll.Managers;
using UnityEngine;
using UnityEngine.Assertions;

namespace _dice_roll
{
    using DieState = Die.Die.DieState;

    /// <summary>
    /// Dummy class to manage everything
    /// TODO add proper manager for handling multiple dices
    /// The above as a consideration for further development
    /// I consider it beyond assumptions of this test
    /// </summary>
    public class RollManager : MonoBehaviour
    {
        [SerializeField] private RigidbodyDie _die;
        [SerializeField] private DiceRollUI _ui;

        private PointsCounter _pointsCounter;

        public static readonly int FaceSpecialValue = -1;

        private void Awake()
        {
            Assert.IsNotNull(_die, "Missing dice reference");
            Assert.IsNotNull(_ui, "Missing ui reference");

            //Initialize gui values
            _ui.Initialize(FaceSpecialValue);

            _die.OnStateChange += OnDieStateChange;

            _pointsCounter = new PointsCounter();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            
            //Testing purpose only
            if (Input.GetKeyDown(KeyCode.Space) && _die.CurrentState == DieState.SettledDown)
            {
                _die.AutoRoll();
            }
        }

        private void OnDieStateChange(DieState dieState)
        {
            switch (dieState)
            {
                case DieState.PickedUp:
                    OnDiePickup();
                    break;
                case DieState.Thrown:
                    break;
                case DieState.SettledDown:
                    OnDieSettleDown();
                    break;
                case DieState.Aborted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dieState), dieState, null);
            }
        }

        private void OnDiePickup()
        {
            _ui.UpdateValues(FaceSpecialValue, _pointsCounter.TotalValue);
        }

        private void OnDieSettleDown()
        {
            //there is no need to check value as we wait for the die to settle down
            _pointsCounter.AddPoints(_die.CheckValue);
            _ui.UpdateValues(_pointsCounter.LastValue, _pointsCounter.TotalValue);

            Debug.Log($"Dice rolled: {_pointsCounter.LastValue}");
        }
    }
}