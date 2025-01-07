using System.ComponentModel;
using System.Text.RegularExpressions;

public enum ArmorTypeId
{
    [Description("Unspecified")]
    Unspecified,
    [Description("Natural Armor")]
    Natural,
    [Description("Leather Armor")]
    Leather,
    [Description("Studded Leather")]
    StuddedLeather,
    [Description("Hide Armor")]
    Hide,
    [Description("Chain Shirt")]
    ChainShirt,
    [Description("Chain Mail")]
    ChainMail,
    [Description("Scale Mail")]
    ScaleMail,
    [Description("Plate")]
    Plate,
    [Description("Other")]
    Other
}

public class Monster
{
    public required string Name;
    public string? Description;
    public string? Alignment;
    public AlignmentGoodness? AlignmentGoodness;
    public AlignmentLawfulness? AlignmentLawfulness;
    public int? HitPointsDefault;
    public string? HitPointsRoll;
    public int? ArmorClass;
    // Basic string representation
    public string? ArmorType;
    // More advanced enum representation used in part 3
    public ArmorTypeId? ArmorTypeId;
    // Full metadata representation used in part 4
    public ArmorMetadata? ArmorMetadata;

    // Returns a string overview of the monster. If extended is true, it will also include the armor metadata.
    public string GetOverview(bool extended = false)
    {
        var armorType = ArmorMetadata == null ? ArmorTypeId.ToString() : ArmorMetadata.DisplayName;
        var lines = new[]
        {
            $"Name: {Name}",
            $"Description: {Description}",
            $"Alignment: {Alignment}",
            $"Hit points roll: {HitPointsRoll}",
            $"Armor class: {ArmorClass}",
            $"Armor type: {armorType}"
        };

        if (extended && ArmorMetadata != null)
        {
            lines = lines.Concat(new[]
            {
                $"Armor category: {ArmorMetadata.Category}",
                $"Armor weight: {ArmorMetadata.Weight}"
            }).ToArray();
        }

        return string.Join(Environment.NewLine, lines);
    }
}

// Extended armor information used in part 4
public class ArmorMetadata
{
    public required string DisplayName;
    public ArmorCategory Category;
    public required string Weight;
}

public enum ArmorCategory
{
    Light,
    Medium,
    Heavy
}

public enum AlignmentGoodness
{
    Unaligned,
    Good,
    Neutral,
    Evil
}

public enum AlignmentLawfulness
{
    Unaligned,
    Lawful,
    Neutral,
    Chaotic
}

public class MonsterManual1 : ScriptInterface
{
    /// <summary>
    /// Parses the monster data from the given path. This function does a lot of the work for many of the missions.
    /// </summary>
    /// <param name="path">The path to the monster data file</param>
    /// <param name="armorMetadataPath">The optional path to the armor metadata file</param>
    /// <returns>A list of monsters</returns>
    public List<Monster> ParseMonsterData(string path, string? armorMetadataPath = null)
    {
        var monsters = new List<Monster>();
        var data = File.ReadAllText(path);

        // Split the data into individual monster entries, as strings to be further parsed in the loop below
        var monsterEntries = data.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

        foreach (var entry in monsterEntries)
        {
            var lines = entry.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 5)
            {
                throw new Exception("Invalid monster entry:\n" + entry);
            }

            // Parse name
            var name = lines[0].Trim();
            var monster = new Monster { Name = name };

            // Parse description and alignment from the second line
            var typeInfo = lines[1].Split(',', 2);
            monster.Description = typeInfo[0].Trim();
            monster.Alignment = ExtractAlignment(typeInfo[1].Trim());
            (monster.AlignmentGoodness, monster.AlignmentLawfulness) = ExtractAlignmentAxes(typeInfo[1].Trim());

            // Parse some of the remaining stats
            foreach (var line in lines.Skip(2))
            {
                if (line.StartsWith("Hit Points:"))
                {
                    // Parse default HP and roll
                    var hpMatch = Regex.Match(line, @"Hit Points: (\d+) \(([^)]+)\)");
                    if (hpMatch.Success)
                    {
                        monster.HitPointsDefault = int.Parse(hpMatch.Groups[1].Value);
                        monster.HitPointsRoll = hpMatch.Groups[2].Value;
                    }
                }
                else if (line.StartsWith("Armor Class:"))
                {
                    // Parse armor class and type
                    var acMatch = Regex.Match(line, @"Armor Class: (\d+)(?:\s*\(([^)]+)\))?");
                    if (acMatch.Success)
                    {
                        monster.ArmorClass = int.Parse(acMatch.Groups[1].Value);
                        monster.ArmorType = acMatch.Groups[2].Value;
                        monster.ArmorTypeId = ParseArmorTypeId(monster.ArmorType);
                    }
                }
            }

            monsters.Add(monster);
        }

        // In part 4, we also need metadata about the armor types. We add it here.
        if (armorMetadataPath != null)
        {
            // Parse the armor metadata file
            var armorData = File.ReadAllText(armorMetadataPath);
            var armorEntries = armorData.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var armorMetadata = new Dictionary<ArmorTypeId, ArmorMetadata>();
            foreach (var entry in armorEntries)
            {
                var lines = entry.Split(',', StringSplitOptions.RemoveEmptyEntries);
                var armorVariant = (ArmorTypeId)Enum.Parse(typeof(ArmorTypeId), lines[0]);
                var armorName = lines[1];
                var armorCategory = lines[2];
                var armorWeight = lines[3];
                armorMetadata[armorVariant] = new ArmorMetadata
                {
                    DisplayName = armorName,
                    Category = (ArmorCategory)Enum.Parse(typeof(ArmorCategory), armorCategory),
                    Weight = armorWeight
                };
            }

            // Update armor metadata for each monster
            foreach (var monster in monsters)
            {
                if (monster.ArmorTypeId.HasValue && armorMetadata.TryGetValue(monster.ArmorTypeId.Value, out var metadata))
                {
                    monster.ArmorMetadata = metadata;
                }
            }
        }

        return monsters;
    }

    /// <summary>
    /// Extracts the alignment from the second line of the monster entry
    /// </summary>
    private string ExtractAlignment(string input)
    {
        var alignmentPattern = @"(lawful|neutral|chaotic)\s+(good|neutral|evil)|neutral|unaligned";
        var match = Regex.Match(input, alignmentPattern, RegexOptions.IgnoreCase);

        return match.Success ? match.Value : input;
    }

    /// <summary>
    /// Extracts the alignment axes from the second line of the monster entry
    /// </summary>
    private (AlignmentGoodness, AlignmentLawfulness) ExtractAlignmentAxes(string input)
    {
        var alignmentPattern = @"(lawful|neutral|chaotic)\s+(good|neutral|evil)|(neutral)";
        var match = Regex.Match(input, alignmentPattern, RegexOptions.IgnoreCase);

        Enum.TryParse<AlignmentLawfulness>(match.Groups[1].Value, true, out var lawfulness);
        Enum.TryParse<AlignmentGoodness>(match.Groups[2].Value, true, out var goodness);

        // Special case for the single word "neutral"
        if (match.Groups[3].Success)
        {
            lawfulness = AlignmentLawfulness.Neutral;
            goodness = AlignmentGoodness.Neutral;
        }

        return (goodness, lawfulness);
    }

    /// <summary>
    /// Helper function to get the description of an armor type
    /// </summary>
    public string GetArmorTypeDescription(ArmorTypeId armorType)
    {
        var field = armorType.GetType().GetField(armorType.ToString());
        var attribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute));
        return attribute?.Description ?? armorType.ToString();
    }

    /// <summary>
    /// Parses the armor type from the input string
    /// </summary>
    public ArmorTypeId ParseArmorTypeId(string input)
    {
        // Try to match the armor type to an enum value
        foreach (ArmorTypeId type in Enum.GetValues(typeof(ArmorTypeId)))
        {
            if (input.Contains(GetArmorTypeDescription(type), StringComparison.OrdinalIgnoreCase))
            {
                return type;
            }
        }

        // No match was found, so we'll call it "Other"
        if (!string.IsNullOrEmpty(input))
        {
            return ArmorTypeId.Other;
        }

        return ArmorTypeId.Unspecified;
    }

    /// <summary>
    /// Runs the script
    /// </summary>
    public void Run()
    {
        var monsters = ParseMonsterData("MonsterManual.txt");

        // Print a few examples to verify parsing
        foreach (var monster in monsters.Take(10))
        {
            Console.WriteLine(monster.GetOverview());
            Console.WriteLine();
        }
    }
}