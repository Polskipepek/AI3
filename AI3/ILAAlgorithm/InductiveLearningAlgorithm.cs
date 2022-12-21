using AI3.Entities;

namespace AI3.ILAAlgorithm {
    internal class InductiveLearningAlgorithm {
        public List<Rule> Learn(IEnumerable<Entity> data) {
            // Step 1: Divide the array (m examples) into n subarrays (n is the number of classes). One subarray for each possible value of the class attribute (for each class), set R as an empty set.
            var ruleset = new List<Rule>();
            var subarrays = new Dictionary<int, List<Entity>>();
            //var combinations = CombinationGenerator.GetCombinations(data.First().Attributes.Select(x => x.Name));

            DivideArrayIntoSubarrays(data, subarrays);

            // Steps 2-8 are repeated for each subarray
            foreach (var subarray in subarrays.Values) {
                int maxCount = 0;
                List<ILAAttribute> maxCombination = new();
                // Step 2: Initialize the number of attribute combinations j as j=1 (set j=1)
                int numAttributeCombinations = 1;

                var otherSubarrays = subarrays.Where(x => !x.Value.Equals(subarray));
                var otherSubarraysAttributes = otherSubarrays.SelectMany(s => s.Value.SelectMany(x => x.Attributes)).DistinctBy(x => x.Value);

                // Step 3: For the considered subarray, divide the list of attributes into distinct combinations, each combination with j distinct attributes
                // Step 4: For each attribute combination, count the number of occurrences of the class attribute  value that appear under this same attribute combination in the unmarked rows of the considered subarray, 
                // but which at the same time do not appear under this same attribute combination in other subarrays. Call the first combination with the maximum number of occurrences as maxCombination
                while (!subarray.All(x => x.IsClassified)) {
                    var currentCombinations = CombinationGenerator.GetCombinations(subarray.First().Attributes.Select(x => x.Name)).Where(x => x.Count() == numAttributeCombinations);

                    if (!currentCombinations.Any()) {
                        break;
                    }

                    foreach (var combination in currentCombinations) {
                        var otherEntities = otherSubarrays.SelectMany(x => x.Value);

                        var rowsThatArentClassified = subarray.Where(x => !x.IsClassified);

                        var rowsWithAttributesThatArentInOtherSubarrays = rowsThatArentClassified
                            .Where(entity =>
                                !otherEntities.Any(e => combination.All(c => e.Attributes.Any(a => a.Name.Equals(c)
                                && a.Value.Equals(entity.Attributes.First(at => at.Name.Equals(c)).Value)))));

                        if (!rowsWithAttributesThatArentInOtherSubarrays.Any()) {
                            continue;
                        }
                        var attributeValues = new Dictionary<IEnumerable<string>, List<object>>() { { combination, new List<object>() } };
                        //var attributeValues = combination.ToDictionary(x => x, x => new List<object>());
                        foreach (var row in rowsWithAttributesThatArentInOtherSubarrays) {
                            attributeValues[combination].Add(row.Attributes.First(a => combination.Any(c => c.Equals(a.Name))).Value);
                        }

                        foreach (var attributeValue in attributeValues) {
                            var groups = attributeValue.Value.GroupBy(y => y).OrderByDescending(g => g.Count()).ToList();
                            for (int i = 0; i < groups.Count; i++) {
                                var count = groups[i].Count();
                                if (count > maxCount) {
                                    maxCombination = new();
                                    maxCombination.AddRange(from name in combination
                                                            select new ILAAttribute() { Name = name, Value = groups[i].Key });
                                    maxCount = count;
                                }
                            }
                        }

                        //foreach (var attributeValue in attributeValues) {
                        //    var key = attributeValue.Value.GroupBy(y => y).OrderByDescending(g => g.Count()).First().Key;
                        //    var groups = attributeValue.Value.GroupBy(y => y).OrderByDescending(g => g.Count()).ToList();
                        //    var count = attributeValue.Value.Count(y => y.Equals(key));
                        //    if (count > maxCount) {
                        //        maxCombination = new();
                        //        maxCombination.AddRange(from name in combination
                        //                                select new ILAAttribute() { Name = name, Value = groups[i].Key });
                        //        maxCount = count;
                        //    }
                        //}
                    }

                    if (!maxCombination.Any()) {
                        // Step 5: If COMBINATION_MAXIMUM=0, increase j by 1 and go to step 3 (j=j+1)
                        numAttributeCombinations++;
                    } else {
                        // Step 6: Mark as classified all rows in the considered subarray where values from maxCombination appear
                        MarkClassified(subarray, maxCombination);
                        // Step 7: Add a rule to R, the left side of this rule contains the names of the attributes from maxCombination with their values,
                        // separated by the logical AND operator, and the right side contains the value of the decision attribute associated with the subarray (class)
                        ruleset.Add(new Rule {
                            Attributes = maxCombination,
                            DecisionAttribute = subarray.First().DecisionAttribute //todo
                        });
                        maxCombination = new();
                        maxCount = 0;
                    }

                    // If there are still unmarked rows, go to step 4
                    // If there are no more subarrays, return the current set of rules R
                }
            }
            return ruleset;
        }

        private static void DivideArrayIntoSubarrays(IEnumerable<Entity> data, Dictionary<int, List<Entity>> subarrays) {
            foreach (var row in data) {
                if (!subarrays.ContainsKey(row.DecisionAttribute)) {
                    subarrays[row.DecisionAttribute] = new List<Entity>();
                }
                subarrays[row.DecisionAttribute].Add(row);
            }
        }

        private static void MarkClassified(List<Entity> subarray, List<ILAAttribute> combination) {
            // Mark all rows in the subarray as classified if they contain all values from the combination.
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
