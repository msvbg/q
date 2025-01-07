public class AlgorithmDesignMission3 : ScriptInterface
{
    /// <summary>
    /// Converts a number to its ordinal form
    /// </summary>
    static string OrdinalNumber(int number)
    {
        int lastDigit = number % 10;
        if (number > 10)
        {
            int secondToLastDigit = number / 10 % 10;
            if (secondToLastDigit == 1)
            {
                return $"{number}th";
            }
        }

        switch (lastDigit)
        {
            case 1: return $"{number}st";
            case 2: return $"{number}nd";
            case 3: return $"{number}rd";
            default: return $"{number}th";
        }
    }

    /// <summary>
    /// Display a few sample ordinal numbers
    /// </summary>
    public void Run()
    {
        Console.WriteLine(OrdinalNumber(0));
        Console.WriteLine(OrdinalNumber(1));
        Console.WriteLine(OrdinalNumber(2));
        Console.WriteLine(OrdinalNumber(3));
        Console.WriteLine(OrdinalNumber(4));
        Console.WriteLine(OrdinalNumber(11));
        Console.WriteLine(OrdinalNumber(12));
        Console.WriteLine(OrdinalNumber(13));
        Console.WriteLine(OrdinalNumber(21));
        Console.WriteLine(OrdinalNumber(22));
        Console.WriteLine(OrdinalNumber(23));
    }
}