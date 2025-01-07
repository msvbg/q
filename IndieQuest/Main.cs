List<ScriptInterface> scripts = new List<ScriptInterface>
{
    new AlgorithmDesignMission1(),
    new AlgorithmDesignMission2(),
    new AlgorithmDesignMission3(),
    new AlgorithmDesignMission4(),
    new MonsterManual1(),
    new MonsterManual2and3Part1(),
    new MonsterManual3Part2(),
    new MonsterManual4(),
    new Regex1(),
    new Regex2(),
    new Regex3(),
    new Regex4(),
};

while (true)
{
    Console.WriteLine("\x1b[1mSelect the script you want to run:\x1b[0m");
    Console.WriteLine("\x1b[31mq. Quit\x1b[0m");
    for (int i = 0; i < scripts.Count; i++)
    {
        Console.WriteLine($"\x1b[32m{i + 1}. {scripts[i].GetType().Name}\x1b[0m");
    }
    Console.Write("\x1b[33mChoice: \x1b[0m");

    string choice = Console.ReadLine() ?? "";
    if (choice == "q")
    {
        break;
    }

    Console.WriteLine();

    ScriptInterface? script = null;
    try
    {
        script = scripts[int.Parse(choice) - 1];
    }
    catch (ArgumentOutOfRangeException)
    {
        Console.WriteLine("\x1b[31mInvalid choice.\x1b[0m");
    }
    catch (FormatException)
    {
        Console.WriteLine("\x1b[31mInvalid choice.\x1b[0m");
    }

    if (script != null)
    {
        script.Run();
    }
    else
    {
        Console.WriteLine("\x1b[31mInvalid choice.\x1b[0m");
    }

    Console.WriteLine();
}