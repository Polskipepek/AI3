using AI3.Entities;

namespace AI3.ILAAlgorithm {
    internal class InductiveLearningAlgorithm {
        public List<Rule> Learn(IEnumerable<Entity> data) {
            var ruleset = new List<Rule>();
            var subarrays = new Dictionary<string, List<Entity>>();
            Dictionary<IEnumerable<ILAAttribute>, int> pairs = new();
            int prevNumOfAttributes = 1;

            DivideArrayIntoSubarrays(data, subarrays);

            foreach (var subarray in subarrays.Values) {
                int maxCount = 0;
                IEnumerable<ILAAttribute> maxCombination = new List<ILAAttribute>();
                int numAttributeCombinations = 1;

                var otherEntities = subarrays.Where(x => !x.Value.Equals(subarray)).SelectMany(x => x.Value);
                var currentCombinations = GetCombinations(subarray, numAttributeCombinations);
                Console.WriteLine($"Learning combination - {string.Join(", ", currentCombinations.SelectMany(x => x).ToList())},\t\tsubarray {subarray.First().DecisionAttribute}, j={numAttributeCombinations}");

                while (!subarray.All(x => x.IsClassified)) {
                    if (prevNumOfAttributes != numAttributeCombinations) {
                        currentCombinations = GetCombinations(subarray, numAttributeCombinations);
                        prevNumOfAttributes = numAttributeCombinations;
                        Console.WriteLine($"Learning combination - {string.Join(", ", currentCombinations.SelectMany(x => x).ToList())},\t\tsubarray {subarray.First().DecisionAttribute}, j={numAttributeCombinations}");
                    }

                    if (!currentCombinations.Any()) break;

                    foreach (var combination in currentCombinations) {
                        pairs.Clear();

                        var entities = subarray
                            .Where(e => !e.IsClassified)
                            .Where(entity =>
                                !otherEntities.Any(otherEntity => combination.All(c => otherEntity.Attributes.Any(a =>
                                    a.Name.Equals(c)
                                    && a.Value.Equals(entity.Attributes.First(at => at.Name.Equals(c)).Value)))));

                        if (entities.Any()) {
                            var localMax = GetMaxCombinationFromEntities(pairs, combination, entities, out int counter);
                            if (counter > maxCount) {
                                maxCombination = localMax;
                                maxCount = counter;
                            }
                        }
                    }

                    if (!maxCombination.Any()) {
                        numAttributeCombinations++;
                    } else {
                        MarkClassified(subarray, maxCombination);
                        CreateRule(ruleset, subarray, maxCombination);
                        maxCombination = Enumerable.Empty<ILAAttribute>();
                        maxCount = 0;
                    }
                }
            }
            return ruleset;
        }

        private static IEnumerable<IEnumerable<string>> GetCombinations(List<Entity> subarray, int numAttributeCombinations) {
            return CombinationGenerator.GetCombinations(subarray.First().Attributes.Select(x => x.Name)).Where(x => x.Count() == numAttributeCombinations);
        }

        private static void CreateRule(List<Rule> ruleset, IEnumerable<Entity> subarray, IEnumerable<ILAAttribute> maxCombination) {
            ruleset.Add(new Rule {
                Attributes = maxCombination.ToList(),
                DecisionAttribute = subarray.First(e => e.Attributes.All(a => !maxCombination.Any(c => c.Name.Equals(a.Name) && c.Value.Equals(a.Name)))).DecisionAttribute
            });
        }

        private static IEnumerable<ILAAttribute> GetMaxCombinationFromEntities(Dictionary<IEnumerable<ILAAttribute>, int> pairs, IEnumerable<string> combination, IEnumerable<Entity> entities, out int counter) {
            foreach (var entity in entities) {
                foreach (var column in combination) {
                    var pair = pairs.FirstOrDefault(a => a.Key.Select(x => x.Name).Contains(column) && a.Key.Select(x => x.Value).Contains(entity.Attributes.FirstOrDefault(a => a.Name.Equals(column))?.Value));
                    if (pair.Key == null) {
                        pairs.Add(combination.Select(c => new ILAAttribute() { Name = c, Value = entity.Attributes.First(a => a.Name.Equals(c)).Value }), 1);
                    } else {
                        pairs[pair.Key]++;
                    }
                }
            }
            var maxPair = pairs.OrderByDescending(x => x.Value).FirstOrDefault();
            counter = maxPair.Value;
            return maxPair.Key;
        }

        private static void DivideArrayIntoSubarrays(IEnumerable<Entity> data, Dictionary<string, List<Entity>> subarrays) {
            foreach (var row in data) {
                if (!subarrays.ContainsKey(row.DecisionAttribute)) {
                    subarrays[row.DecisionAttribute] = new List<Entity>();
                }
                subarrays[row.DecisionAttribute].Add(row);
            }
        }

        private static void MarkClassified(IEnumerable<Entity> subarray, IEnumerable<ILAAttribute> combination) {
            foreach (var entity in subarray) {
                bool found = true;
                foreach (var attribute in combination) {
                    if (!entity.Attributes.Contains(attribute)) {
                        found = false;
                        break;
                    }
                }
                if (found) {
                    entity.IsClassified = true;
                }
            }
        }
    }
}
