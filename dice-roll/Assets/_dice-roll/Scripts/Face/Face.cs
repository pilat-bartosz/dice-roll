using UnityEngine;

namespace _dice_roll.Dice
{
    
    public class Face : MonoBehaviour, IFace<int>
    {
        [Range(0, 10000)] [SerializeField] protected int _value;
        
        public int Value => _value;
        
        /// <summary>
        /// Should be in "up" direction for the result.
        /// </summary>
        public virtual Vector3 FaceDirection => transform.forward;
    }
}