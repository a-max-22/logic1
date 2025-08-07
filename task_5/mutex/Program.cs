
class Program
{
    private static Mutex _logMutex = new Mutex(false, "LogMutex");
    private static string _logFilePath = "log.txt";

    static void Main(string[] args)
    {
        for (int i = 0; i < 5; i++)
        {
            ThreadPool.QueueUserWorkItem(LogMessage, i);
        }
        Thread.Sleep(2000);
    }

    static void LogMessage(object state)
    {
        int threadId = (int)state;
        try
        {
            bool acquired = _logMutex.WaitOne(TimeSpan.FromSeconds(1), false);

            if (acquired)
            {
                using (StreamWriter writer = new StreamWriter(_logFilePath, true))
                {
                    writer.WriteLine($"Thread {threadId} logged a message at {DateTime.Now}");
                    Thread.Sleep(100);
                }

                _logMutex.ReleaseMutex();
            }
            else
            {
                Console.WriteLine($"Err: timeout. Thread {threadId} could not acquire the mutex");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Err: Thread {threadId} : {ex.Message}");
        }
    }
}
