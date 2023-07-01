using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace _dice_roll.Dice
{
    public class Dice : MonoBehaviour, IDice
    {
        protected DiceState CurrentState;
        
        [SerializeField] private List<Face> _faces;
        
        public Action<DiceState> OnStateChange;
        
        public enum DiceState
        {
            PickedUp,
            Thrown,
            SettleDown
        }
        
        private void Awake()
        {
            Assert.IsNotNull(_faces, "shouldn't be null");
            Assert.IsTrue(_faces.Count > 0, "Dice should have faces");
        }
        
        protected void ChangeState(DiceState newState)
        {
            CurrentState = newState;
            OnStateChange?.Invoke(newState);
        }
        
        public int CheckValue
        {
            get
            {
                if (CurrentState != DiceState.SettleDown) return -1;
                
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