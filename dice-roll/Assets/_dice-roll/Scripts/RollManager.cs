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
    /// </summary>
    public class RollManager : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private RigidbodyDie _die;
        [SerializeField] private DiceRollUI _ui;
        [SerializeField] private Bounds _playgroundBounds;

        private PointsCounter _pointsCounter;
        
        /// <summary>
        /// Plane on which the player's hand will move when grabbing a dice.
        /// </summary>
        private Plane _handPlane;

        public static readonly int FaceSpecialValue = -1;

        private void Awake()
        {
            Assert.IsNotNull(_camera, "Missing camera reference");
            Assert.IsNotNull(_die, "Missing dice reference");
            Assert.IsNotNull(_ui, "Missing ui reference");

            //Initialize gui values
            _ui.Initialize(FaceSpecialValue);

            _die.OnStateChange += OnDieStateChange;

            //Initialize hand plane
            _handPlane = new Plane(Vector3.up, -3f);

            _pointsCounter = new PointsCounter();
        }

        private void FixedUpdate()
        {
            if (_die.CurrentState == DieState.PickedUp)
            {
                var ray = _camera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

                if (_handPlane.Raycast(ray, out var newZ))
                {
                    var newPoint = ray.GetPoint(newZ);

                    if (_playgroundBounds.Contains(newPoint))
                    {
                        _die.UpdatePosition(newPoint);
                    }
                }
            }
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

            //Try Grab on LMB 
            if (Input.GetMouseButtonDown(0))
            {
                var screenRay = _camera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

                if (Physics.Raycast(screenRay, out var hitInfo, 100f))
                {
                    if (hitInfo.transform == _die.transform)
                    {
                        //Grab
                        _die.Grab();
                    }
                }
            }

            //Release if picked up
            if (Input.GetMouseButtonUp(0) && _die.CurrentState == DieState.PickedUp)
            {
                _die.Release();
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
            //there is no need to check value as we wait to die to settle down
            _pointsCounter.AddPoints(_die.CheckValue);
            _ui.UpdateValues(_pointsCounter.LastValue, _pointsCounter.TotalValue);

            Debug.Log($"Dice rolled: {_pointsCounter.LastValue}");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(_playgroundBounds.center, _playgroundBounds.size);
        }
    }
}