using AverageCalculator;


namespace AverageCalculator.Tests
{

    [TestClass]
    public class AverageCalculatorTests
    {
        private AverageCalculator _calculator;

        [TestInitialize]
        public void Initialize()
        {
            _calculator = new AverageCalculator();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateAverage_WithEmptyArray_ThrowsArgumentException()
        {
            
            int[] numbers = new int[0];
            
            _calculator.CalculateAverage(numbers);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateAverage_WithNullArray_ThrowsArgumentException()
        {
            
            int[] numbers = null;

            _calculator.CalculateAverage(numbers);
        }

        [TestMethod]
        public void CalculateAverage_WithSingleElement_ReturnsSameElement()
        {
            
            int[] numbers = { 5 };
            
            double result = _calculator.CalculateAverage(numbers);

            Assert.AreEqual(5.0, result);
        }

        [TestMethod]
        public void CalculateAverage_WithMultipleElements_ReturnsCorrectAverage()
        {
            
            int[] numbers = { 1, 2, 3, 4, 5 };
            
            double result = _calculator.CalculateAverage(numbers);
            
            Assert.AreEqual(3.0, result);
        }

        [TestMethod]
        public void CalculateAverage_WithNegativeNumbers_ReturnsCorrectAverage()
        {
            
            int[] numbers = { -1, -2, -3, -4, -5 };

            double result = _calculator.CalculateAverage(numbers);
            
            Assert.AreEqual(-3.0, result);
        }

        [TestMethod]
        public void CalculateAverage_WithMixedNumbers_ReturnsCorrectAverage()
        {
            
            int[] numbers = { -1, 0, 1, 2, 3 };
            
            double result = _calculator.CalculateAverage(numbers);
            
            Assert.AreEqual(1.0, result);
        }

        [TestMethod]
        public void CalculateAverage_WithLargeNumbers_ReturnsCorrectAverage()
        {
            
            int[] numbers = { int.MaxValue, int.MinValue };

            double result = _calculator.CalculateAverage(numbers);
            
            Assert.AreEqual(-0.5, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateAverage_WithLargeNumbersOverflow_ThrowsException()
        {
            int[] numbers = { int.MaxValue, int.MaxValue };

            double result = _calculator.CalculateAverage(numbers);
        }

    }
}