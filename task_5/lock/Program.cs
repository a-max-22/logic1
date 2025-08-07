

class Program
{
    private static int _counter = 0;
    private static readonly object _lockObject = new object();
    private static int _maxIncrementsCount = 10;
    private static int _numberOfThreads = 10;

    static async Task Main(string[] args)
    {
        List<Task> tasks = new List<Task>();

        for (int i = 0; i < _numberOfThreads; i++)
        {
            tasks.Add(Task.Run(() => IncrementCounter(i)));
        }

        await Task.WhenAll(tasks);

        Console.WriteLine($"Final counter value: {_counter}");
        Console.WriteLine("All tasks have completed.");
    }

    static void IncrementCounter(int threadId)
    {
        try
        {
            for (int i = 0; i < _maxIncrementsCount; i++)
            {
                lock (_lockObject)
                {
                    _counter++;
                }

                Thread.Sleep(10);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Err in thread {threadId} encountered an error: {ex.Message}");
        }
    }
}
