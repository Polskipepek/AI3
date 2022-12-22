using AI3.Entities;

namespace AI3.ILAAlgorithm {
    internal class InductiveLearningAlgorithm {
        public List<Rule> Learn(IEnumerable<Entity> data) {
            var ruleset = new List<Rule>();
            var subarrays = new Dictionary<string, List<Entity>>();

            DivideArrayIntoSubarrays(data, subarrays);
            Dictionary<IEnumerable<ILAAttribute>, int> pairs = new();

            foreach (var subarray in subarrays.Values) {
                int maxCount = 0;
                List<ILAAttribute> maxCombination = new();
                int numAttributeCombinations = 1;

                var otherSubarrays = subarrays.Where(x => !x.Value.Equals(subarray));
                var otherSubarraysAttributes = otherSubarrays.SelectMany(s => s.Value.SelectMany(x => x.Attributes)).DistinctBy(x => x.Value);
                var otherEntities = otherSubarrays.SelectMany(x => x.Value);
                int prevNumOfAttributes = 0;

                var currentCombinations = CombinationGenerator.GetCombinations(subarray.First().Attributes.Select(x => x.Name)).Where(x => x.Count() == numAttributeCombinations);


                while (!subarray.All(x => x.IsClassified)) {
                    if (prevNumOfAttributes != numAttributeCombinations) {
                        currentCombinations = CombinationGenerator.GetCombinations(subarray.First().Attributes.Select(x => x.Name)).Where(x => x.Count() == numAttributeCombinations);
                        prevNumOfAttributes = numAttributeCombinations;
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


                        foreach (var entity in entities) {
                            foreach (var column in combination) {
                                var pair = pairs.FirstOrDefault(a => a.Key.Select(x => x.Name).Contains(column) && a.Key.Select(x => x.Value).Contains(entity.Attributes.FirstOrDefault(a => a.Name.Equals(column))?.Value));
                                if (pair.Key == null) {
                                    pairs.Add(combination.Select(c => new ILAAttribute() { Name = c, Value = entity.Attributes.First(a => a.Name.Equals(c)).Value }), 1);
                                } else {
                                    pairs[pair.Key]++;
                                }
                            }

                            var bestLocalCombination = pairs.OrderByDescending(x => x.Value).First();

                            if (bestLocalCombination.Value > maxCount && entities.Any()) {
                                maxCount = bestLocalCombination.Value;
                                maxCombination = combination.Select(name => new ILAAttribute { Name = name, Value = bestLocalCombination.Key.First(a => a.Name.Equals(name)).Value }).ToList();
                            }
                        }
                    }

                    if (!maxCombination.Any()) {
                        numAttributeCombinations++;
                    } else {
                        MarkClassified(subarray, maxCombination);
                        ruleset.Add(new Rule {
                            Attributes = maxCombination,
                            DecisionAttribute = subarray.First(e => e.Attributes.All(a => !maxCombination.Any(c => c.Name.Equals(a.Name) && c.Value.Equals(a.Name)))).DecisionAttribute
                        });
                        maxCombination = new();
                        maxCount = 0;
                    }
                }
            }
            return ruleset;
        }

        private static void DivideArrayIntoSubarrays(IEnumerable<Entity> data, Dictionary<string, List<Entity>> subarrays) {
            foreach (var row in data) {
                if (!subarrays.ContainsKey(row.DecisionAttribute)) {
                    subarrays[row.DecisionAttribute] = new List<Entity>();
                }
                subarrays[row.DecisionAttribute].Add(row);
            }
        }

        private static void MarkClassified(List<Entity> subarray, List<ILAAttribute> combination) {
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
