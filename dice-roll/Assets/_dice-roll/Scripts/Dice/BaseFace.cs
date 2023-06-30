using UnityEngine;

namespace DiceRoll
{
    public abstract class BaseFace : MonoBehaviour
    {
        /// <summary>
        /// Value of the face
        /// </summary>
        public abstract int Value { get; }

        /// <summary>
        /// Should be in "up" direction for the result.
        /// </summary>
        public abstract Vector3 FaceDirection { get; }
    }
}