using AI3.Entities;
using AI3.ILAAlgorithm;
using AI3.Mappers;
using System.Diagnostics;

Console.WriteLine("Cwiczenie 3!");
Console.WriteLine();
while (true) {
    Console.WriteLine("1. Example Data from presentation.");
    Console.WriteLine("2. Test Banking Data.");
    Console.WriteLine("3. Training Banking Data.");
    Console.WriteLine("4. Test Fruits Data.TODO");
    Console.WriteLine("5. Training Fruits Data.TODO");
    Console.WriteLine("6. Custom Data");

    var input = Console.ReadKey();
    var data = GetData(input.Key);

    Console.Clear();
    Console.WriteLine($"There are {data.Count()} entities. Do you want to limit data? (Y - for yes; Any other key to continue with {data.Count()} entities.)");
    var limitFlag = Console.ReadKey().Key;
    Console.Clear();
    if (limitFlag == ConsoleKey.Y) {
        Console.WriteLine($"Number of entities (1-{data.Count()}): ");
        var entitycount = Console.ReadLine();
        if (int.TryParse(entitycount, out int result)) {
            if (result < data.Count() && result > 0) data = data.Take(result);
        }
    }
    Console.Clear();

    var ila = new InductiveLearningAlgorithm();
    var stopwatch = new Stopwatch();

    stopwatch.Start();
    var rules = ila.Learn(data);
    stopwatch.Stop();

    Console.WriteLine($"Entities: {data.Count()}");
    Console.WriteLine($"Stopwatch: {stopwatch.ElapsedMilliseconds}ms");
    Console.WriteLine("Rules:");

    for (int i = 0; i < rules.Count; i++) {
        Console.Write($"Rule {i + 1:000}: ");
        foreach (var att in rules[i].Attributes) {
            Console.Write($"({att.Name} = {att.Value})");
        }
        Console.WriteLine($"\tDecision = {rules[i].DecisionAttribute}");
    }

    Console.ReadKey();
    Console.Clear();
}
IEnumerable<Entity> GetData(ConsoleKey key) {
    if (key == ConsoleKey.D2) {
        var data = DataReader.ReadData($"{Environment.CurrentDirectory}/Data/test - short.csv", ";");
        return TableToEntitiesMapper.Map(data);
    } else if (key == ConsoleKey.D3) {
        var data = DataReader.ReadData($"{Environment.CurrentDirectory}/Data/train.csv", ";");
        return TableToEntitiesMapper.Map(data);
        //} else if (key == ConsoleKey.D4) {

        //} else if (key == ConsoleKey.D5) {

    } else if (key == ConsoleKey.D6) {
        Console.Clear();
        Console.WriteLine("Decision attribute column name needs to be named \'decision\'");
        Console.WriteLine("Enter full path to the file. c:/users/alfa/desktop/dummydata.csv");
        var path = Console.ReadLine();
        Console.WriteLine("Enter delimeter type: (\',\', \';\'");
        var delimeter = Console.ReadKey();
        var data = DataReader.ReadData(path, delimeter.KeyChar.ToString());
        return TableToEntitiesMapper.Map(data);

    } else {
        var examples = new List<Entity> {
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "sredni"), new ILAAttribute("Kolor", "niebieski"), new ILAAttribute("Ksztalt", "kostka") }, DecisionAttribute = "TAK" },
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "maly"), new ILAAttribute("Kolor", "czerwony"), new ILAAttribute("Ksztalt", "klin") }, DecisionAttribute = "NIE"},
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "maly"), new ILAAttribute("Kolor", "czerwony"), new ILAAttribute("Ksztalt", "kula") }, DecisionAttribute = "TAK" },
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "duzy"), new ILAAttribute("Kolor", "czerwony"), new ILAAttribute("Ksztalt", "klin") }, DecisionAttribute = "NIE"},
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "duzy"), new ILAAttribute("Kolor", "zielony"), new ILAAttribute("Ksztalt", "slup") }, DecisionAttribute = "TAK" },
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "duzy"), new ILAAttribute("Kolor", "czerwony"), new ILAAttribute("Ksztalt", "slup") }, DecisionAttribute = "NIE"},
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "duzy"), new ILAAttribute("Kolor", "zielony"), new ILAAttribute("Ksztalt", "kula") }, DecisionAttribute = "TAK" }
        };

        return examples;
    }

    //DataReader.ReadData();

    return null;
}