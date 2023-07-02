using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace _dice_roll.Die
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyDie : Die, IPickable
    {
        [Header("Die settings")] [SerializeField]
        private bool _autoRollOnStart = true;

        [SerializeField] private float _throwAbortVelocitySqr = 1f;
        [SerializeField] private float _throwTorqueForceMultiplier = 1f;

        [Header("Auto-roll settings")] 
        [SerializeField] private float _autoRollUpForce = 0.1f;
        [SerializeField] private float _autoRollTorqueForce = 10f;

        private Rigidbody _rigidbody;

        private Vector3 _dragToPosition;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            Assert.IsNotNull(_rigidbody, "shouldn't be null");
        }

        private void Start()
        {
            if (_autoRollOnStart)
            {
                AutoRoll();
            }
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

        public void AutoRoll()
        {
            if (CurrentState is DieState.SettledDown or DieState.Aborted)
            {
                ChangeState(DieState.Thrown);
                _rigidbody.AddForce(Vector3.up * _autoRollUpForce, ForceMode.Impulse);
                _rigidbody.AddRelativeTorque(Random.onUnitSphere * _autoRollTorqueForce, ForceMode.Impulse);
            }
        }
    }
}