using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

//TODO split into Dice and Rigidbody dice
//TODO add drag and release
namespace _dice_roll.Dice
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyDice : Dice
    {
        private Rigidbody _rigidbody;

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

        private void Update()
        {
            if (CurrentState == DiceState.Thrown && _rigidbody.IsSleeping())
            {
                ChangeState(DiceState.SettleDown);
            }
        }


        //TODO move to external source
        private Plane _plane;

        private void OnMouseDown()
        {
            ChangeState(DiceState.PickedUp);

            _rigidbody.isKinematic = true;

            _plane = new Plane(Vector3.up, -2f);
        }

        private void OnMouseDrag()
        {
            var ray = Camera.main.ScreenPointToRay(
                new Vector3(
                    Input.mousePosition.x,
                    Input.mousePosition.y,
                    0f)
            );

            if (_plane.Raycast(ray, out var newZ))
            {
                var newPoint = ray.GetPoint(newZ);


                _rigidbody.MovePosition(newPoint);
            }
        }

        private void OnMouseUp()
        {
            ChangeState(DiceState.Thrown);

            _rigidbody.isKinematic = false;

            //Add random torque when velocity is small enough
            //this is to simulate rolling a dice in place instead throwing it across 
            //this also prevents picking up and placing it again with same value
            _rigidbody.AddRelativeTorque(
                _rigidbody.velocity.sqrMagnitude > 1f
                    ? _rigidbody.velocity
                    : Random.onUnitSphere * 10
            );
        }

        public void AutoRoll()
        {
            ChangeState(DiceState.Thrown);
            _rigidbody.AddForce(Vector3.up/10, ForceMode.Impulse);
            _rigidbody.AddTorque(Random.onUnitSphere * 10);
        }
    }
}