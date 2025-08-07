
class Program
{
    static async Task Main(string[] args)
    {
        string[] filePaths = { "numbers1.txt", "numbers2.txt", "numbers3.txt" };

        List<Task<List<int>>> tasks = new List<Task<List<int>>>();
        foreach (var filePath in filePaths)
        {
            tasks.Add(ReadNumbersFromFileAsync(filePath));
        }

        List<int>[] numberLists = await Task.WhenAll(tasks);

        List<int> allNumbers = new List<int>();
        foreach (var numberList in numberLists)
        {
            allNumbers.AddRange(numberList);
        }

        double median = CalculateMedian(allNumbers);

        Console.WriteLine($"The median of all numbers from files  is: {median}");
    }

    static async Task<List<int>> ReadNumbersFromFileAsync(string filePath)
    {
        try
        {
            string[] lines = await File.ReadAllLinesAsync(filePath);

            List<int> numbers = new List<int>();
            foreach (var line in lines)
            {
                if (int.TryParse(line, out int number))
                {
                    numbers.Add(number);
                }
            }

            return numbers;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Err: read file {filePath}: {ex.Message}");
            return new List<int>();
        }
    }

    static double CalculateMedian(List<int> numbers)
    {
        numbers.Sort();

        int count = numbers.Count;
        if (count % 2 == 1)
        {
            return numbers[count / 2];
        }
        else
        {
            return (numbers[(count - 1) / 2] + numbers[count / 2]) / 2.0;
        }
    }
}
