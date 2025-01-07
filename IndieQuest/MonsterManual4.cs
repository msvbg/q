public class MonsterManual4 : ScriptInterface
{
    private MonsterManual1 monsterManual1 = new MonsterManual1();

    public void Run()
    {
        var monsters = monsterManual1.ParseMonsterData("MonsterManual.txt", "ArmorTypes.txt");
        MonsterManual3Part2 monsterManual3Part2 = new MonsterManual3Part2(monsters, true);

        monsterManual3Part2.Run();
    }
}