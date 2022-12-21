using AI3.Entities;
using AI3.ILAAlgorithm;
using AI3.Mappers;
using System.Diagnostics;

Console.WriteLine("Cwiczenie 3!");
Console.WriteLine();
while (true) {
    Console.WriteLine("1. Example Data from presentation.");
    Console.WriteLine("2. Test Data.");
    Console.WriteLine("3. Training Data.");
    var input = Console.ReadKey();
    var data = GetData(input.Key);
    Console.Clear();

    var ila = new InductiveLearningAlgorithm();
    var stopwatch = new Stopwatch();

    stopwatch.Start();
    var rules = ila.Learn(data);
    stopwatch.Stop();

    Console.WriteLine($"Stopwatch: {stopwatch.ElapsedMilliseconds}ms");
    Console.WriteLine("Rules:");

    for (int i = 0; i < rules.Count; i++) {
        Console.Write($"Rule {i + 1}: ");
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
        var mapped = TableToEntitiesMapper.Map(data);
        return mapped;
    } else if (key == ConsoleKey.D3) {
        var data = DataReader.ReadData($"{Environment.CurrentDirectory}/Data/train.csv");
        var mapped = TableToEntitiesMapper.Map(data);
        return mapped;
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