public class AlgorithmDesignMission4 : ScriptInterface
{
    /// <summary>
    /// Repaired Matrix-like effect
    /// </summary>
    public void Run()
    {
        Random random = new Random();
        var streams = new List<int>();
        var symbols = "!@#$%^&*()_+-=[];',.\\/~{}:|<>?";

        for (int i = 0; i < 10; i++) streams.Add(random.Next(0, 80));

        Console.ForegroundColor = ConsoleColor.DarkGreen;

        while (true)
        {
            for (int x = 0; x < 80; x++)
            {
                Console.Write(streams.Contains(x) ? symbols[random.Next(symbols.Length)] : ' ');
            }

            Console.WriteLine();
            Thread.Sleep(100);

            if (random.Next(3) == 0) streams.RemoveAt(random.Next(streams.Count));
            if (random.Next(3) == 0) streams.Add(random.Next(0, 80));
        }
    }
}