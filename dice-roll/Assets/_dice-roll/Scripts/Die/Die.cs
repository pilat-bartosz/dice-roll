using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace _dice_roll.Die
{
    public class Die : MonoBehaviour, IDie
    {
        public DieState CurrentState { get; private set; }

        [SerializeField] private List<Face.Face> _faces;

        public Action<DieState> OnStateChange;

        public enum DieState
        {
            PickedUp,
            Thrown,
            SettledDown,
            Aborted
        }

        private void Awake()
        {
            Assert.IsNotNull(_faces, "shouldn't be null");
            Assert.IsTrue(_faces.Count > 0, "Dice should have faces");
        }

        protected void ChangeState(DieState newState)
        {
            CurrentState = newState;
            OnStateChange?.Invoke(newState);
        }

        public int CheckValue
        {
            get
            {
                if (CurrentState != DieState.SettledDown) return -1;

                var value = 0;
                var bestMatch = float.MinValue;

                foreach (var face in _faces)
                {
                    var dot = Vector3.Dot(Vector3.up, face.FaceDirection);

                    //1 is max and best value in this case
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
}