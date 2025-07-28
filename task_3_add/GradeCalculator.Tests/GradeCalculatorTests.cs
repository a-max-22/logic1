using GradeCalculator;


namespace GradeCalculator.Tests
{

    [TestClass]
    public class GradeCalculator
    {
        private GradeCalculator _calculator;

        [TestInitialize]
        public void Initialize()
        {
            _calculator = new GradeCalculator();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateAverage_WithEmptyArray_ThrowsArgumentException()
        {
            
            int[] grades = new int[0];
            
            _calculator.CalculateAverage(grades);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateAverage_WithNullArray_ThrowsArgumentException()
        {
            
            int[] grades = null;

            _calculator.CalculateAverage(grades);
        }

        [TestMethod]
        public void CalculateAverage_WithSingleElement_ReturnsSameElement()
        {
            
            int[] grades = { 5 };
            
            double result = _calculator.CalculateAverage(grades);

            Assert.AreEqual(5.0, result);
        }

        [TestMethod]
        public void CalculateAverage_WithMultipleElements_ReturnsCorrectAverage()
        {
            
            int[] grades = { 1, 2, 3, 4, 5 };
            
            double result = _calculator.CalculateAverage(grades);
            
            Assert.AreEqual(3.0, result);
        }

        [TestMethod]
        public void CalculateAverage_WithNegativegrades_ReturnsCorrectAverage()
        {
            
            int[] grades = { -1, -2, -3, -4, -5 };

            double result = _calculator.CalculateAverage(grades);
            
            Assert.AreEqual(-3.0, result);
        }

        [TestMethod]
        public void CalculateAverage_WithMixedgrades_ReturnsCorrectAverage()
        {
            
            int[] grades = { -1, 0, 1, 2, 3 };
            
            double result = _calculator.CalculateAverage(grades);
            
            Assert.AreEqual(1.0, result);
        }

        [TestMethod]
        public void CalculateAverage_WithLargegrades_ReturnsCorrectAverage()
        {
            
            int[] grades = { int.MaxValue, int.MinValue };

            double result = _calculator.CalculateAverage(grades);
            
            Assert.AreEqual(-0.5, result);
        }

        [TestMethod]
        public void CalculateAverage_WithLargegradesPossibleOverflow_ReturnsCorrectAverage()
        {
            int[] grades = { int.MaxValue - 1, int.MaxValue - 1 };

            double result = _calculator.CalculateAverage(grades);
            Assert.AreEqual(int.MaxValue - 1, result);
        }

    }
}