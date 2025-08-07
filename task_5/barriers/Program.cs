using System.Collections.Concurrent;

class Program
{
    private static int _numberOfThreads = 5;
    private static ConcurrentBag<int> _symbolsCounts = new ConcurrentBag<int>();
    private static Barrier _barrier = new Barrier(_numberOfThreads, b => CalcSymbolsCountTotal());

    private static int _totalSymbolsCount;
    static async Task Main(string[] args)
    {
        _totalSymbolsCount = 0;

        List<Task> tasks = new List<Task>();

        for (int i = 0; i < _numberOfThreads; i++)
        {
            string fileName = $"input.{i}.txt";
            FillFile(fileName);
        }

        for (int i = 0; i < _numberOfThreads; i++)
        {
            string fileName = $"input.{i}.txt";
            tasks.Add(Task.Run(() => GetSymbolsCount(fileName)));
        }

        await Task.WhenAll(tasks);

        Console.WriteLine($"Total input files symbols count excluding newlines is : {_totalSymbolsCount}");
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
    static void GetSymbolsCount(object state)
    {
        string fileName = (string)state;

            using (StreamReader reader = new StreamReader(fileName, true))
            {
                int symCountExcludingNewlines = 0;
                while (reader.Peek() > 0)
                {
                    string l = reader.ReadLine();
                    if (l is null)
                    {
                        break;
                    }; 
                    symCountExcludingNewlines += l.Length;
                }
                _symbolsCounts.Add(symCountExcludingNewlines);
            }
            _barrier.SignalAndWait();
    }

    static void CalcSymbolsCountTotal()
    {
        foreach (int count in _symbolsCounts)
        {
            _totalSymbolsCount += count;
        }
    }
}
