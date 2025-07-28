using System;

namespace GradeCalculator 
{
    public class GradeCalculator 
    {
        public double CalculateAverage(int[] grades)
        {
            if (grades == null || grades.Length == 0)
            {
                throw new ArgumentException("ERR: array must not be null or empty.");
            }

            double sum = 0.0;
            foreach (int grade in grades)
            {
                checked
                {
                    sum += ((double)grade / grades.Length);
                }
            }

            return sum;
        }
    }
}
