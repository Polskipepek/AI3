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
                var otherEntities = otherSubarrays.SelectMany(x => x.Value);

                while (!subarray.All(x => x.IsClassified)) {
                    var currentCombinations = CombinationGenerator.GetCombinations(subarray.First().Attributes.Select(x => x.Name)).Where(x => x.Count() == numAttributeCombinations);

                    if (!currentCombinations.Any()) {
                        break;
                    }

                    foreach (var combination in currentCombinations) {
                        var groupedEntities = subarray
                            .Where(entity =>
                                !otherEntities.Any(e => combination.All(c => e.Attributes.Any(a => a.Name.Equals(c)
                                && a.Value.Equals(entity.Attributes.First(at => at.Name.Equals(c)).Value)))))
                            .GroupBy(e => combination.All(c => e.Attributes.Any(a => a.Name.Equals(c) && a.Value.Equals(e.Attributes.First(at => at.Name.Equals(c)).Value)))).ToList();
                        //.GroupBy(e => e.Attributes.Where(a => combination.Contains(a.Name)).Select(a => new { a.Name, a.Value }));

                        // Count the occurrences of each combination and match with all combinations
                        foreach (var group in groupedEntities) {
                            var combinationCount = group.Count();

                            if (combinationCount > maxCount && group.Any()) {
                                maxCount = combinationCount;
                                maxCombination = combination.Select(name => new ILAAttribute { Name = name, Value = group.First().Attributes.First(a => a.Name.Equals(name)).Value }).ToList();
                            }
                        }
                    }


                    //foreach (var combination in currentCombinations) {
                    //    // Group the attributes in the subarray by their name and value
                    //    var groupedAttributes = subarray
                    //        .Where(x => !x.IsClassified)
                    //        .Where(entity =>
                    //           !otherEntities.Any(e => combination.All(c => e.Attributes.Any(a => a.Name.Equals(c)
                    //           && a.Value.Equals(entity.Attributes.First(at => at.Name.Equals(c)).Value)))))
                    //        .SelectMany(e => e.Attributes)
                    //        .Where(a => combination.Contains(a.Name))
                    //        .GroupBy(a => new { a.Name, a.Value });

                    //    // Count the occurrences of each name/value pair and match with all combinations
                    //    foreach (var group in groupedAttributes) {
                    //        var valueCount = group.Count();
                    //        //var entitiesWithAttributesThatArentInOtherSubarrays = group.Where(attribute =>
                    //        //    !otherSubarrays.SelectMany(x => x.Value).Any(e => combination.All(c => e.Attributes.Any(a => a.Name.Equals(c)
                    //        //    && a.Value.Equals(attribute.Value)))));

                    //        if (valueCount > maxCount && group.Any()) {
                    //            maxCount = valueCount;
                    //            maxCombination = combination.Select(name => new ILAAttribute { Name = name, Value = groupedAttributes.First(a => a.Select(g => g.Name).Equals(name)).Key.Value }).ToList();
                    //        }
                    //    }
                    //}

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
