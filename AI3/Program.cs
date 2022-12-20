using AI3.Entities;
using AI3.ILAAlgorithm;
using System.Drawing;

Console.WriteLine("Cwiczenie 3!");

var data = GetData(true);

var ila = new InductiveLearningAlgorithm();
var xd = ila.Learn(data);

//Console.WriteLine(xd);
//string[] inputSet = { "Size", "Color", "Shape" };
//var aa = CombinationGenerator.GetCombinations(inputSet).OrderBy(x => x.Count());
//foreach (var combination in aa) {
//    Console.WriteLine(string.Join(", ", combination));
//}

Console.ReadLine();


IEnumerable<Entity> GetData(bool @default) {
    if (@default) {
        var examples = new List<Entity> {
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "sredni"), new ILAAttribute("Kolor", "niebieski"), new ILAAttribute("Ksztalt", "kostka") }, decisionAttribute = 1 },
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "maly"), new ILAAttribute("Kolor", "czerwony"), new ILAAttribute("Ksztalt", "klin") }, decisionAttribute = 0},
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "maly"), new ILAAttribute("Kolor", "czerwony"), new ILAAttribute("Ksztalt", "kula") }, decisionAttribute = 1 },
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "duzy"), new ILAAttribute("Kolor", "czerwony"), new ILAAttribute("Ksztalt", "klin") }, decisionAttribute = 0},
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "duzy"), new ILAAttribute("Kolor", "zielony"), new ILAAttribute("Ksztalt", "slup") }, decisionAttribute = 1 },
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "duzy"), new ILAAttribute("Kolor", "czerwony"), new ILAAttribute("Ksztalt", "slup") }, decisionAttribute = 0},
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "duzy"), new ILAAttribute("Kolor", "zielony"), new ILAAttribute("Ksztalt", "kula") }, decisionAttribute = 1 }
        };

        return examples;
    }

    //DataReader.ReadData();

    return null;
}