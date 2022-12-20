using AI3.Entities;

namespace AI3.ILAAlgorithm {
    internal class InductiveLearningAlgorithm {
        public List<Rule> Learn(IEnumerable<Entity> data) {
            // Step 1: Divide the array (m examples) into n subarrays (n is the number of classes). One subarray for each possible value of the class attribute (for each class), set R as an empty set.
            var ruleset = new List<Rule>();
            var subarrays = new Dictionary<int, List<Entity>>();
            var combinations = CombinationGenerator.GetCombinations(data.First().Attributes.Select(x => x.Name));

            DivideArrayIntoSubarrays(data, subarrays);

            // Steps 2-8 are repeated for each subarray
            foreach (var subarray in subarrays.Values) {
                int maxCount = 0;
                List<ILAAttribute> maxCombination = new();
                // Step 2: Initialize the number of attribute combinations j as j=1 (set j=1)
                int numAttributeCombinations = 1;
                var currentCombinations = combinations.Where(x => x.Count() == numAttributeCombinations);

                var otherSubarrays = subarrays.Where(x => !x.Value.Equals(subarray));
                var otherSubarraysAttributes = otherSubarrays.SelectMany(s => s.Value.SelectMany(x => x.Attributes)).DistinctBy(x => x.Value);

                // Step 3: For the considered subarray, divide the list of attributes into distinct combinations, each combination with j distinct attributes
                // Step 4: For each attribute combination, count the number of occurrences of the class attribute  value that appear under this same attribute combination in the unmarked rows of the considered subarray, 
                // but which at the same time do not appear under this same attribute combination in other subarrays. Call the first combination with the maximum number of occurrences as COMBINATION_MAXIMUM
                while (!subarray.All(x => x.IsClassified)) {
                    if (!currentCombinations.Any()) {
                        break;
                    }

                    foreach (var combination in currentCombinations) {
                        var rowsThatArentClassified = subarray.Where(x => !x.IsClassified);
                        var rowsWithAttributesThatArentInOtherSubarrays = rowsThatArentClassified
                            .Where(entity => !otherSubarrays.Any(a => a.Value
                                    .Any(entity2 => entity2.Attributes.Any(att => combination.Any(c => c.Equals(att.Name))
                                    && entity.Attributes.Any(a => a.Name.Equals(att.Name) && a.Value.Equals(att.Value))))))
                            .Where(entity => combination.All(c => entity.Attributes.Any(a => a.Name.Equals(c))));

                        if (!rowsWithAttributesThatArentInOtherSubarrays.Any()) {
                            continue;
                        }

                        var attributeValues = combination.ToDictionary(x => x, x => new List<object>());
                        foreach (var row in rowsWithAttributesThatArentInOtherSubarrays) {
                            foreach (var attribute in combination) {
                                attributeValues[attribute].Add(row.Attributes.First(a => a.Name.Equals(attribute)).Value);
                            }
                        }

                        foreach (var attributeValue in attributeValues) {
                            var localValue = attributeValue.Value.GroupBy(y => y).OrderByDescending(g => g.Count()).First().Key;
                            var count = attributeValue.Value.Count(y => y.Equals(localValue));
                            if (count > maxCount) {
                                maxCombination = new();
                                maxCombination.AddRange(from name in combination
                                                        select new ILAAttribute() { Name = name, Value = localValue });
                                maxCount = count;
                            }
                        }
                    }

                    if (!maxCombination.Any()) {
                        // Step 5: If COMBINATION_MAXIMUM=0, increase j by 1 and go to step 3 (j=j+1)
                        numAttributeCombinations++;
                    } else {
                        // Step 6: Mark as classified all rows in the considered subarray where values from COMBINATION_MAXIMUM appear
                        MarkClassified(subarray, maxCombination);
                    }

                    // Step 7: Add a rule to R, the left side of this rule contains the names of the attributes from COMBINATION_MAXIMUM with their values,
                    // separated by the logical AND operator, and the right side contains the value of the decision attribute associated with the subarray (class)
                    ruleset.Add(new Rule {
                        Attributes = maxCombination,
                        DecisionAttribute = subarray.First().DecisionAttribute
                    });

                    // If there are still unmarked rows, go to step 4
                    // If there are no more subarrays, return the current set of rules R
                }
                // Step 8: If all rows in the considered subarray are marked as classified, then go to processing the next subarray (go to step 2)
                //if (subarray.All(x => x.IsClassified)) {
                //    break;
                //}
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
                foreach (var attribute in combination) {
                    if (!entity.Attributes.Contains(attribute)) {
                        break;
                    }
                }
                entity.IsClassified = true;
            }
        }
    }
}
