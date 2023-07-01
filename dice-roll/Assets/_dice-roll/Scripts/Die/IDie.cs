namespace _dice_roll.Die
{
    public interface IDie
    {
        /// <summary>
        /// Returns a value of the face that "face up" the best.
        /// </summary>
        /// <returns>Value of the face</returns>
        public int CheckValue { get; }

    }
}