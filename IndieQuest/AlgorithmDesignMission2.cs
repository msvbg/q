using System;
using System.Collections.Generic;

public class AlgorithmDesignMission2 : ScriptInterface
{
    /// <summary>
    /// Joins a list of strings with "and" or "and,"
    /// </summary>
    /// <param name="items">The list of strings to join</param>
    /// <param name="useSerialComma">Whether to use "and," instead of "and"</param>
    /// <returns>The joined string</returns>
    public static string JoinWithAnd(List<string> items, bool useSerialComma = true)
    {
        if (items.Count == 0)
        {
            return string.Empty;
        }

        if (items.Count == 1)
        {
            return items[0];
        }

        if (items.Count == 2)
        {
            return $"{items[0]} and {items[1]}";
        }

        // Create a copy of the items
        var copiedItems = new List<string>(items);

        if (useSerialComma)
        {
            // Prepend "and " to the last item
            copiedItems[^1] = "and " + copiedItems[^1];
        }
        else
        {
            // Join the last two items with " and "
            string lastTwo = $"{copiedItems[^2]} and {copiedItems[^1]}";
            copiedItems[^2] = lastTwo;
            copiedItems.RemoveAt(copiedItems.Count - 1);
        }

        // Return the copied list joined with ", "
        return string.Join(", ", copiedItems);
    }

    /// <summary>
    /// Entry point for AlgorithmDesignMission2
    /// </summary>
    public void Run()
    {
        var items = new List<string> { "Jazlyn", "Theron", "Dayana", "Rolando" };

        Console.WriteLine("Items, joined with serial comma:");
        var copiedItems = new List<string>(items);
        while (copiedItems.Count > 0)
        {
            Console.WriteLine(JoinWithAnd(copiedItems));
            copiedItems.RemoveAt(0);
        }

        Console.WriteLine();

        Console.WriteLine("Items, joined without serial comma:");
        while (items.Count > 0)
        {
            Console.WriteLine(JoinWithAnd(items, false));
            items.RemoveAt(0);
        }
    }
}

