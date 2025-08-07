
class Program
{
    private static CountdownEvent _countdown;

    static void Main(string[] args)
    {
        int numberOfThreads = 5;
        _countdown = new CountdownEvent(numberOfThreads);

        for (int i = 0; i < numberOfThreads; i++)
        {
            string fileName = $"input.{i}.txt";
            FillFile(fileName);
        }
        
        for (int i = 0; i < numberOfThreads; i++)
        {
            string fileName = $"input.{i}.txt";
            ThreadPool.QueueUserWorkItem(PerformTask, fileName);
        }

        _countdown.Wait();

        Console.WriteLine("All sources were read");
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }

    static void FillFile(string fileName)
    {
        int numLines = 6;
        for (int i = 0; i < numLines; i++)
        {
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine($"file data {DateTime.Now}");
            }
        }
    }
    static void PerformTask(object state)
    {
        string fileName = (string)state;
        try
        {
            using (StreamReader reader = new StreamReader(fileName, true))
            {
                while (reader.Peek() > 0)
                {
                    string l = reader.ReadLine();
                    Console.WriteLine(l);
                }
            }
            _countdown.Signal();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Err: reading  {fileName} : {ex.Message}");
            _countdown.Signal();
        }
    }
}
