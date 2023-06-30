using UnityEngine;

namespace DiceRoll
{
    public class Face : BaseFace
    {
        [Range(0, 10000)] [SerializeField] protected int _value;
        public override int Value => _value;

        public override Vector3 FaceDirection => transform.forward;
    }
}