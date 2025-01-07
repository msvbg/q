// Contains solution for Monster manual 3 part 2

public class MonsterManual3Part2 : ScriptInterface
{
    public List<Monster> monsters;
    private MonsterManual1 monsterManual1 = new();
    private MonsterManual2and3Part1 monsterManual2and3Part1;

    private bool useExtendedArmorOverview;

    /// <summary>
    /// Loads the default monster list from the MonsterManual.txt file
    /// </summary>
    private List<Monster> LoadDefaultMonsterList()
    {
        return monsterManual1.ParseMonsterData("MonsterManual.txt");
    }

    /// <summary>
    /// Constructor for MonsterManual3Part2
    /// </summary>
    /// <param name="monsters">The list of monsters to use. If null, the default monster list will be used.</param>
    /// <param name="useExtendedArmorOverview">Whether to use the extended armor overview when printing monsters</param>
    public MonsterManual3Part2(List<Monster>? monsters = null, bool useExtendedArmorOverview = false)
    {
        this.monsters = monsters ?? LoadDefaultMonsterList();
        this.monsterManual2and3Part1 = new MonsterManual2and3Part1(monsters, useExtendedArmorOverview);
        this.useExtendedArmorOverview = useExtendedArmorOverview;
    }

    /// <summary>
    /// Searches for monsters by armor type ID
    /// </summary>
    private List<Monster> SearchByArmorTypeId(ArmorTypeId armorType)
    {
        return monsters
            .Where(monster => monster.ArmorTypeId == armorType)
            .ToList();
    }

    /// <summary>
    /// Helper function to request a number from the user
    /// </summary>
    private void RequestNumber(out int choice)
    {
        int.TryParse(Console.ReadLine() ?? "0", out choice);
        choice -= 1; // Account for 0-based index
        Console.WriteLine();
    }

    /// <summary>
    /// Searches for monsters by armor type
    /// </summary>
    public void SearchByArmorType()
    {
        // Prompt the user for a search query
        Console.WriteLine("Which armor type do you want to display?");
        int armorTypeIndex = 1;
        foreach (ArmorTypeId armorType in Enum.GetValues(typeof(ArmorTypeId)))
        {
            Console.WriteLine($"  {armorTypeIndex++}: {monsterManual1.GetArmorTypeDescription(armorType)}");
        }

        Console.WriteLine();
        int armorTypeChoice = -1;
        while (armorTypeChoice < 0 || armorTypeChoice >= Enum.GetValues(typeof(ArmorTypeId)).Length)
        {
            Console.WriteLine("Enter number:");
            RequestNumber(out armorTypeChoice);
        }

        var matching = SearchByArmorTypeId((ArmorTypeId)armorTypeChoice);

        int monsterChoice;
        if (matching.Count == 0)
        {
            Console.WriteLine("No monsters were found.");
            return;
        }
        else if (matching.Count == 1)
        {
            // If there is only one matching monster, select it by default
            monsterChoice = 0;
        }
        else
        {
            Console.WriteLine("Which monster did you want to look up?");
            int monsterIndex = 1;
            foreach (var monster in matching)
            {
                Console.WriteLine($"  {monsterIndex++}: {monster.Name}");
            }

            RequestNumber(out monsterChoice);

            // If no monsters were found, ask the user to try again
            while (monsterChoice < 0 || monsterChoice >= matching.Count)
            {
                Console.WriteLine("No monsters were found. Try again:");
                RequestNumber(out monsterChoice);
            }
        }

        // Display the information for the chosen monster
        Console.WriteLine("Displaying information for {0}:", matching[monsterChoice].Name);
        Console.WriteLine();
        Console.WriteLine(matching[monsterChoice].GetOverview(useExtendedArmorOverview));
    }

    /// <summary>
    /// Entry point for MonsterManual3Part2
    /// </summary>
    public void Run()
    {
        Console.WriteLine("MONSTER MANUAL");
        Console.WriteLine();

        // Prompt the user for a search option
        string choice = "";
        while (choice != "n" && choice != "a")
        {
            Console.WriteLine("Do you want to search by (n)ame or (a)rmor type?");
            choice = Console.ReadLine() ?? "";
            Console.WriteLine();
        }

        if (choice == "n")
        {
            // This part was already covered in the previous exercise.
            monsterManual2and3Part1.SearchByName();
        }
        else
        {
            SearchByArmorType();
        }
    }
}
