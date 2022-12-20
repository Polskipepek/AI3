using AI3.Entities;
using AI3.ILAAlgorithm;

Console.WriteLine("Cwiczenie 3!");

var data = GetData(true);

var ila = new InductiveLearningAlgorithm();
var xd = ila.Learn(data);

Console.WriteLine(string.Join(",", xd));
//string[] inputSet = { "Size", "Color", "Shape" };
//var aa = CombinationGenerator.GetCombinations(inputSet).OrderBy(x => x.Count());
//foreach (var combination in aa) {
//    Console.WriteLine(string.Join(", ", combination));
//}

Console.ReadLine();


IEnumerable<Entity> GetData(bool @default) {
    if (@default) {
        var examples = new List<Entity> {
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "sredni"), new ILAAttribute("Kolor", "niebieski"), new ILAAttribute("Ksztalt", "kostka") }, DecisionAttribute = 1 },
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "maly"), new ILAAttribute("Kolor", "czerwony"), new ILAAttribute("Ksztalt", "klin") }, DecisionAttribute = 0},
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "maly"), new ILAAttribute("Kolor", "czerwony"), new ILAAttribute("Ksztalt", "kula") }, DecisionAttribute = 1 },
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "duzy"), new ILAAttribute("Kolor", "czerwony"), new ILAAttribute("Ksztalt", "klin") }, DecisionAttribute = 0},
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "duzy"), new ILAAttribute("Kolor", "zielony"), new ILAAttribute("Ksztalt", "slup") }, DecisionAttribute = 1 },
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "duzy"), new ILAAttribute("Kolor", "czerwony"), new ILAAttribute("Ksztalt", "slup") }, DecisionAttribute = 0},
            new Entity { Attributes = new List<ILAAttribute>() { new ILAAttribute("Rozmiar", "duzy"), new ILAAttribute("Kolor", "zielony"), new ILAAttribute("Ksztalt", "kula") }, DecisionAttribute = 1 }
        };

        return examples;
    }

    //DataReader.ReadData();

    return null;
}