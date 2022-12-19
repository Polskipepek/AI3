using AI3.Entities;
using AI3.ILAAlgorithm;

Console.WriteLine("Cwiczenie 3!");

var data = GetData(true);


var xd = InductiveLearningAlgorithm.Learn(data);
Console.WriteLine(xd);



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