using UnityEngine;

namespace _dice_roll
{
    /// <summary>
    /// Interface for drag & drop in 3D space
    /// </summary>
    public interface IPickable
    {
        public void PickUp();
        public void DragTo(Vector3 newPosition);
        public void Drop();
    }
}