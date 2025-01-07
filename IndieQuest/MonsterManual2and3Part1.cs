// Contains solution for Monster manual 2 and part 1 of Monster manual 3
public class MonsterManual2and3Part1 : ScriptInterface
{
    private List<Monster> monsters;
    private bool useExtendedArmorOverview;

    /// <summary>
    /// Entry point for MonsterManual2and3Part1
    /// </summary>
    public void Run()
    {
        Console.WriteLine("MONSTER MANUAL");
        Console.WriteLine();

        SearchByName();
    }

    /// <summary>
    /// Constructor for MonsterManual2and3Part1. Optionally, a custom list of monsters can be provided.
    /// </summary>
    /// <param name="monsters">The list of monsters to use</param>
    /// <param name="useExtendedArmorOverview">Whether to use the extended armor overview when printing monsters</param>
    public MonsterManual2and3Part1(List<Monster>? monsters = null, bool useExtendedArmorOverview = false)
    {
        this.monsters = monsters ?? LoadDefaultMonsterList();
        this.useExtendedArmorOverview = useExtendedArmorOverview;
    }

    /// <summary>
    /// Loads the default monster list from the MonsterManual.txt file. This is used by the pre-part 4 scripts.
    /// </summary>
    private List<Monster> LoadDefaultMonsterList()
    {
        return new MonsterManual1().ParseMonsterData("MonsterManual.txt");
    }

    /// <summary>
    /// Searches for monsters by name
    /// </summary>
    private List<Monster> FilterMonstersByName(string query)
    {
        return monsters
            .Where(monster => monster.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    /// <summary>
    /// Searches for monsters by name
    /// </summary>
    public void SearchByName()
    {
        // Prompt the user for a search query
        Console.WriteLine("Enter a query to search monsters by name:");
        string query = Console.ReadLine() ?? "";
        Console.WriteLine();
        List<Monster> matching = FilterMonstersByName(query);

        // If no monsters were found, ask the user to try again
        while (matching.Count == 0)
        {
            Console.WriteLine("No monsters were found. Try again:");
            query = Console.ReadLine() ?? "";
            matching = FilterMonstersByName(query);
            Console.WriteLine();
        }

        int choice = -1;
        if (matching.Count == 1)
        {
            // If there is only one matching monster, select it by default
            choice = 0;
        }
        else
        {
            // Display the list of matching monsters
            Console.WriteLine("Which monster did you want to look up?");
            foreach (var monster in matching)
            {
                Console.WriteLine($"  {matching.IndexOf(monster) + 1}: {monster.Name}");
            }
            Console.WriteLine();

            // Request a number from the user until they enter a valid number
            while (choice < 0 || choice >= matching.Count)
            {
                Console.WriteLine("Enter number:");
                int.TryParse(Console.ReadLine() ?? "0", out choice);
                choice -= 1; // Account for 0-based index
                Console.WriteLine();
            }
        }

        // Display the information for the chosen monster
        Console.WriteLine("Displaying information for {0}:", matching[choice].Name);
        Console.WriteLine();
        Console.WriteLine(matching[choice].GetOverview(useExtendedArmorOverview));
    }
}
