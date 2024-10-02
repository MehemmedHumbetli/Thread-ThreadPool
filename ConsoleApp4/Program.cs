
public class Program
{
    private static CancellationTokenSource cancellationTokenSource;

    static void Main(string[] args)
    {
        Console.Write("Choose the option\n [1]Single / [2]Multi --> ");
        int sin_mul_choice = Convert.ToInt32(Console.ReadLine());

        cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = cancellationTokenSource.Token;

        if (sin_mul_choice == 1)
        {
            Console.WriteLine("Enter the number of JSON files to create: ");
            int fileCount = Convert.ToInt32(Console.ReadLine());
            var thread = new Thread(() =>
            {
                CreateMultipleJsonFiles(fileCount, token);
            });
            thread.Start();
        }
        else if (sin_mul_choice == 2)
        {
            Console.WriteLine("Enter the number of JSON files to create: ");
            int fileCount = Convert.ToInt32(Console.ReadLine());

            for (int i = 1; i <= fileCount; i++)
            {
                Console.Write($"Enter name for file {i}: ");
                string fileName = Console.ReadLine();

                ThreadPool.QueueUserWorkItem(x => CreateJsonFile(fileName, token));
            }
        }

        Console.WriteLine("Press 'c' to cancel the operation...");
        while (Console.ReadKey(true).KeyChar != 'c') { }

        Cancel();
    }

    static void CreateJsonFile(string fileName, CancellationToken token)
    {
        string jsonData = $"{{ \"file\": \"{fileName}\" }}";
        File.WriteAllText($"{fileName}.json", jsonData);
        Console.WriteLine($"Created: {fileName}.json");
    }

    static void CreateMultipleJsonFiles(int count, CancellationToken token)
    {
        for (int i = 1; i <= count; i++)
        {
            Console.Write($"Enter name for file {i}: ");
            string fileName = Console.ReadLine();

            if (token.IsCancellationRequested)
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }

            CreateJsonFile(fileName, token);
        }
    }

    static void Cancel()
    {
        cancellationTokenSource.Cancel();
        Console.WriteLine("Cancellation requested.");
    }
}
