using System;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace _dice_roll.Die
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyDie : Die, IPickable
    {
        [SerializeField] private float _throwAbortVelocitySqr = 1f;
        [SerializeField] private float _throwTorqueForceMultiplier = 1f;

        private Rigidbody _rigidbody;

        private Vector3 _dragToPosition;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            Assert.IsNotNull(_rigidbody, "shouldn't be null");
        }

        private void Start()
        {
            //auto roll on start for randomness
            AutoRoll();
        }

        private void FixedUpdate()
        {
            if (CurrentState == DieState.PickedUp)
            {
                _rigidbody.MovePosition(_dragToPosition);
            }
        }

        private void Update()
        {
            if (CurrentState == DieState.Thrown && _rigidbody.IsSleeping())
            {
                ChangeState(DieState.SettledDown);
            }
        }

        public void PickUp()
        {
            ChangeState(DieState.PickedUp);

            _rigidbody.isKinematic = true;
            
            //Reset target position to prevent flickering/uncontrollable jumps
            _dragToPosition = _rigidbody.position;
        }

        public void DragTo(Vector3 newPoint)
        {
            _dragToPosition = newPoint;
        }

        public void Drop()
        {
            ChangeState(DieState.Thrown);

            _rigidbody.isKinematic = false;
            
            //Abort throw when velocity is too small
            if (_rigidbody.velocity.sqrMagnitude > _throwAbortVelocitySqr)
            {
                _rigidbody.AddRelativeTorque(_rigidbody.velocity * _throwTorqueForceMultiplier);
            }
            else
            {
                ChangeState(DieState.Aborted);
            }
        }

        //Testing purpose only
        public void AutoRoll()
        {
            ChangeState(DieState.Thrown);
            _rigidbody.AddForce(Vector3.up / 10, ForceMode.Impulse);
            _rigidbody.AddTorque(Random.onUnitSphere * 10);
        }
    }
}