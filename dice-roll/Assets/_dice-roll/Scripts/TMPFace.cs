using TMPro;
using UnityEngine;

namespace DiceRoll
{
    [RequireComponent(typeof(TextMeshPro))]
    public class TMPFace : Face
    {
        //tmp forward is faced inside the die so it needs to be revered
        public override Vector3 FaceDirection => -transform.forward;

        private void OnValidate()
        {
            var tmp = GetComponent<TextMeshPro>();
            tmp.text = _value.ToString();
        }
    }
}