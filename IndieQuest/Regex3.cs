using System.Text.RegularExpressions;

public class Regex3 : ScriptInterface
{
    /// <summary>
    /// Entry point for Regex3
    /// </summary>
    public void Run()
    {
        Mission1();
        Console.WriteLine();
        Mission2();
    }

    /// <summary>
    /// Mission 1: Monsters with alignment, the regex way
    /// </summary>
    private void Mission1()
    {
        Console.WriteLine("Mission 1");

        var monsters = new MonsterManual1().ParseMonsterData("MonsterManual.txt");

        var namesByAlignment = new List<string>[3, 3];
        var namesOfUnaligned = new List<string>();
        var namesOfAnyAlignment = new List<string>();
        var namesOfSpecialCases = new List<string>();

        // Initialize the namesByAlignment array
        for (int axis1 = 0; axis1 < 3; axis1++)
        {
            for (int axis2 = 0; axis2 < 3; axis2++)
            {
                namesByAlignment[axis1, axis2] = new List<string>();
            }
        }

        // Categorize the monsters by alignment
        foreach (var monster in monsters)
        {
            var axis1 = (int)(monster.AlignmentLawfulness ?? (int)AlignmentLawfulness.Unaligned);
            var axis2 = (int)(monster.AlignmentGoodness ?? (int)AlignmentGoodness.Unaligned);

            namesOfAnyAlignment.Add(monster.Name);

            if (axis1 == 0 && axis2 == 0 && monster.Alignment == "unaligned")
            {
                namesOfUnaligned.Add(monster.Name);
            }
            else if (axis1 == 0 || axis2 == 0)
            {
                // The alignment was not "unaligned", but also not one of the basic axis values
                namesOfSpecialCases.Add($"{monster.Name} ({monster.Alignment})");
            }
            else
            {
                // Subtract one since the first enum variant is not present in the array
                namesByAlignment[axis1 - 1, axis2 - 1].Add(monster.Name);
            }
        }

        Console.WriteLine($"Names of any alignment: {string.Join(", ", namesOfAnyAlignment)}");
        Console.WriteLine();
        Console.WriteLine($"Names of unaligned: {string.Join(", ", namesOfUnaligned)}");
        Console.WriteLine();
        Console.WriteLine($"Names of special cases: {string.Join(", ", namesOfSpecialCases)}");
        Console.WriteLine();

        for (int axis1 = 0; axis1 < 3; axis1++)
        {
            for (int axis2 = 0; axis2 < 3; axis2++)
            {
                // Add one to the enum values since the first enum variant is not present in the array
                Console.WriteLine($"Monsters with alignment {Enum.GetName(typeof(AlignmentLawfulness), axis1 + 1)} {Enum.GetName(typeof(AlignmentGoodness), axis2 + 1)} are:");
                Console.WriteLine(string.Join(", ", namesByAlignment[axis1, axis2]));
                Console.WriteLine();
            }
        }
    }

    /// <summary>
    /// Adding capture groups to the dice notation regex
    /// </summary>
    private void Mission2()
    {
        Console.WriteLine("Mission 2");

        var diceNotation = new Regex("^([1-9][0-9]*)[dD]([1-9][0-9]*)$");
        var inputs = new[]
        {
            "1d20",
            "6d6",
            "9321d43",
            "1d2",
            "1d",
            "d1",
            "1d20d",
        };

        foreach (var input in inputs)
        {
            var match = diceNotation.Match(input);
            if (match.Success == true)
            {
                Console.WriteLine($"Parsed notation \"{input}\" as rolling {match.Groups[1].Value} dice with {match.Groups[2].Value} sides");
            }
            else
            {
                Console.WriteLine($"Failed to parse notation \"{input}\"");
            }
        }
    }
}
