public class Regex1 : ScriptInterface
{
    /// <summary>
    /// Regex1: Parses and prints monster names
    /// </summary>
    public void Run()
    {
        var monsters = new MonsterManual1().ParseMonsterData("MonsterManual.txt");
        Console.WriteLine("Monsters in the manual are:");
        foreach (var monster in monsters)
        {
            Console.WriteLine(monster.Name);
        }
    }
}