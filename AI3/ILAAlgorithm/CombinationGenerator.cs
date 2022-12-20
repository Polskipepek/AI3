namespace AI3.ILAAlgorithm {
    public class CombinationGenerator {
        public static IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> items) {
            int itemCount = items.Count();
            for (int i = 0; i < (1 << itemCount); i++) {
                List<T> combination = new();
                for (int j = 0; j < itemCount; j++) {
                    if ((i & (1 << j)) != 0) {
                        combination.Add(items.ElementAt(j));
                    }
                }
                yield return combination;
            }
        }
    }
}