using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

//TODO split into Dice and Rigidbody dice
//TODO add drag and release
namespace DiceRoll
{
    public class Dice : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        [SerializeField] private List<TMPFace> _faces;

        private bool _itWasThrown;


        private void Start()
        {
            Assert.IsNotNull(_faces, "shouldn't be null");
            Assert.IsTrue(_faces.Count > 0, "Dice should have faces");

            _rigidbody = GetComponent<Rigidbody>();

            Assert.IsNotNull(_rigidbody, "shouldn't be null");

            _itWasThrown = true;

            //TODO remove/change
            _rigidbody.rotation = Random.rotation;
        }

        private void Update()
        {
            if (_itWasThrown && _rigidbody.IsSleeping())
            {
                _itWasThrown = false;

                Debug.Log(_rigidbody.IsSleeping() + CheckValue().ToString());
            }
        }


        /// <summary>
        /// Returns a value of the face that "face up" the best.
        /// </summary>
        /// <returns>Value of the face</returns>
        private int CheckValue()
        {
            var value = 0;
            var bestMatch = float.MinValue;

            foreach (var face in _faces)
            {
                var dot = Vector3.Dot(Vector3.up, face.FaceDirection);

                //1 is max and best value
                if (dot > bestMatch)
                {
                    value = face.Value;
                    bestMatch = dot;
                }
            }

            return value;
        }
    }
}