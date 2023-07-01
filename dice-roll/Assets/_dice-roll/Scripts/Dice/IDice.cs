namespace _dice_roll.Dice
{
    public interface IDice
    {
        /// <summary>
        /// Returns a value of the face that "face up" the best.
        /// </summary>
        /// <returns>Value of the face</returns>
        public int CheckValue { get; }

    }
}