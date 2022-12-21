using AI3.Entities;

namespace AI3.ILAAlgorithm {
    internal class InductiveLearningAlgorithm {
        public List<Rule> Learn(IEnumerable<Entity> data) {
            var ruleset = new List<Rule>();
            var subarrays = new Dictionary<string, List<Entity>>();

            DivideArrayIntoSubarrays(data, subarrays);

            foreach (var subarray in subarrays.Values) {
                int maxCount = 0;
                List<ILAAttribute> maxCombination = new();
                int numAttributeCombinations = 1;

                var otherSubarrays = subarrays.Where(x => !x.Value.Equals(subarray));
                var otherSubarraysAttributes = otherSubarrays.SelectMany(s => s.Value.SelectMany(x => x.Attributes)).DistinctBy(x => x.Value);

                while (!subarray.All(x => x.IsClassified)) {
                    var currentCombinations = CombinationGenerator.GetCombinations(subarray.First().Attributes.Select(x => x.Name)).Where(x => x.Count() == numAttributeCombinations);

                    if (!currentCombinations.Any()) {
                        break;
                    }

                    foreach (var combination in currentCombinations) {
                        var otherEntities = otherSubarrays.SelectMany(x => x.Value);
                        var entitiesThatArentClassified = subarray.Where(x => !x.IsClassified);

                        var entitiesWithAttributesThatArentInOtherSubarrays = entitiesThatArentClassified
                            .Where(entity =>
                                !otherEntities.Any(e => combination.All(c => e.Attributes.Any(a => a.Name.Equals(c)
                                && a.Value.Equals(entity.Attributes.First(at => at.Name.Equals(c)).Value)))));

                        if (!entitiesWithAttributesThatArentInOtherSubarrays.Any()) {
                            continue;
                        }

                        var attributeGroups = entitiesWithAttributesThatArentInOtherSubarrays.SelectMany(x => x.Attributes)
                            .Where(a => combination.Contains(a.Name))
                            .GroupBy(x => new { x.Name, x.Value })
                            .Select(g => new { Attribute = g.Key, Counter = g.Count() })
                            .OrderByDescending(x => x.Counter);

                        var maxedAttributeGroup = attributeGroups.Where(a => a.Counter == attributeGroups.Max(g => g.Counter));

                        var attributeCombination = maxedAttributeGroup.Select(x => new ILAAttribute { Name = x.Attribute.Name, Value = x.Attribute.Value }).ToList();

                        if (attributeCombination.Count > maxCount) {
                            maxCombination = attributeCombination;
                            maxCount = attributeCombination.Count;
                        }
                    }

                    if (!maxCombination.Any()) {
                        numAttributeCombinations++;
                    } else {
                        MarkClassified(subarray, maxCombination);
                        ruleset.Add(new Rule {
                            Attributes = maxCombination,
                            DecisionAttribute = subarray.First().DecisionAttribute //todo
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
