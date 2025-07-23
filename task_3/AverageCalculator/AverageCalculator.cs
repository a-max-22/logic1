using System;

namespace AverageCalculator
{
    public class AverageCalculator
    {
        public double CalculateAverage(int[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
            {
                throw new ArgumentException("ERR: array must not be null or empty.");
            }

            int sum = 0;
            foreach (int number in numbers)
            {
                sum += number;
            }

            return (double)sum / numbers.Length;
        }
    }
}
