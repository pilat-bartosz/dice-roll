namespace _dice_roll.Face
{
    public interface IFace<T>
    {
        /// <summary>
        /// Value of the face
        /// </summary>
        public T Value { get; }
    }
}