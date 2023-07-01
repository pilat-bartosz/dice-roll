namespace _dice_roll.Managers
{
    public class PointsCounter
    {
        public int LastValue { get; private set; }
        public int TotalValue { get; private set; }

        public void AddPoints(int points)
        {
            LastValue = points;
            TotalValue += points;
        }
    }
}