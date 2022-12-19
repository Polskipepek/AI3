using AI3.Entities;
using AI3.Extentions;

namespace AI3.ILAAlgorithm {
    public static class InductiveLearningAlgorithm {

        public static List<Rule> Learn(IEnumerable<Entity> data) {
            // Step 1: Divide the array (m examples) into n subarrays (n is the number of classes). 
            // One subarray for each possible value of the class attribute (for each class), set R as an empty set.
            var ruleset = new List<Rule>();
            var subarrays = new Dictionary<int, List<Entity>>();

            foreach (var row in data) {
                if (!subarrays.ContainsKey(row.decisionAttribute)) {
                    subarrays[row.decisionAttribute] = new List<Entity>();
                }
                subarrays[row.decisionAttribute].Add(row);
            }

            // Steps 2-8 are repeated for each subarray
            foreach (var subarray in subarrays.Values) {
                // Step 2: Initialize the number of attribute combinations j as j=1 (set j=1)
                int numAttributeCombinations = 1;

                // Step 3: For the considered subarray, divide the list of attributes into distinct combinations, 
                // each combination with j distinct attributes
                var attributeCombinations = GetAttributeCombinations(subarray, numAttributeCombinations);

                while (attributeCombinations.Any()) {
                    // Step 4: For each attribute combination, count the number of occurrences of the class attribute 
                    // value that appear under this same attribute combination in the unmarked rows of the considered subarray, 
                    // but which at the same time do not appear under this same attribute combination in other subarrays. 
                    // Call the first combination with the maximum number of occurrences as COMBINATION_MAXIMUM
                    var maxCombination = GetMaxCombination(subarray, attributeCombinations, subarrays);

                    // Step 5: If COMBINATION_MAXIMUM=0, increase j by 1 and go to step 3 (j=j+1)
                    if (maxCombination == null) {
                        numAttributeCombinations++;
                        attributeCombinations = GetAttributeCombinations(subarray, numAttributeCombinations);
                        continue;
                    }

                    // Step 6: Mark as classified all rows in the considered subarray where values from COMBINATION_MAXIMUM appear
                    MarkClassified(subarray, maxCombination);

                    // Step 7: Add a rule to R, the left side of this rule contains the names of the attributes from COMBINATION_MAXIMUM 
                    // with their values, separated by the logical AND operator, and the right side contains the value of the decision attribute 
                    // associated with the subarray (class)
                    ruleset.Add(new Rule {
                        Attributes = maxCombination,
                        DecisionAttribute = subarray.First().decisionAttribute
                    });

                    // Step 8: If all rows in the considered subarray are marked as classified, then go to processing the next subarray (go to step 2)
                    if (subarray.All(x => x.IsClassified)) {
                        break;
                    }

                    // If there are still unmarked rows, go to step 4
                    // If there are no more subarrays, return the current set of rules R
                }
            }
            return ruleset;
        }

        public static List<ILAAttribute> GetMaxCombination(List<Entity> subarray, List<List<ILAAttribute>> attributeCombinations, Dictionary<int, List<Entity>> subarrays) {
            // Find the combination with the maximum number of occurrences in the subarray and the minimum number of occurrences in the other subarrays.
            var maxCombination = new List<ILAAttribute>();
            var maxCount = 0;
            var minOtherCount = int.MaxValue;
            foreach (var combination in attributeCombinations) {
                var count = 0;
                var otherCount = 0;
                foreach (var row in subarray) {
                    if (row.Attributes.All(a => combination.Contains(a))) {
                        count++;
                    }
                }
                foreach (var otherSubarray in subarrays.Values) {
                    if (otherSubarray == subarray) continue;
                    foreach (var otherRow in otherSubarray) {
                        if (otherRow.Attributes.All(a => combination.Contains(a))) {
                            otherCount++;
                        }
                    }
                }
                if (count > maxCount || (count == maxCount && otherCount < minOtherCount)) {
                    maxCombination = combination;
                    maxCount = count;
                    minOtherCount = otherCount;
                }
            }
            return maxCombination;
        }

        public static List<List<ILAAttribute>> GetAttributeCombinations(List<Entity> subarray, int numAttributeCombinations) {
            var attributeCombinations = new List<List<ILAAttribute>>();
            foreach (var entity in subarray) {
                // Generate all combinations of `numAttributeCombinations` attributes for each entity
                var entityAttributeCombinations = GenerateAttributeCombinations(entity.Attributes.ToList(), numAttributeCombinations);
                attributeCombinations.AddRange(entityAttributeCombinations);
            }
            return attributeCombinations;
        }

        private static List<List<ILAAttribute>> GenerateAttributeCombinations(List<ILAAttribute> attributes, int numAttributes) {
            var combinations = new List<List<ILAAttribute>>();
            // Generate all combinations of `numAttributes` attributes using recursion
            GenerateAttributeCombinationsRecursive(attributes, numAttributes, new List<ILAAttribute>(), combinations);
            return combinations;
        }

        private static void GenerateAttributeCombinationsRecursive(List<ILAAttribute> attributes, int numAttributes, List<ILAAttribute> currentCombination, List<List<ILAAttribute>> combinations) {
            if (currentCombination.Count == numAttributes) {
                combinations.Add(new List<ILAAttribute>(currentCombination));
                return;
            }
            for (int i = 0; i < attributes.Count; i++) {
                currentCombination.Add(attributes[i]);
                GenerateAttributeCombinationsRecursive(attributes, numAttributes, currentCombination, combinations);
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }

        public static List<ILAAttribute> GetMaxCombination(List<Entity> subarray, List<List<ILAAttribute>> combinations) {
            // Find the combination with the maximum number of occurrences in the subarray.
            var maxCombination = new List<ILAAttribute>();
            var maxCount = 0;
            foreach (var combination in combinations) {
                var count = 0;
                foreach (var row in subarray) {
                    if (row.Attributes.All(a => combination.Contains(a))) {
                        count++;
                    }
                }
                if (count > maxCount) {
                    maxCombination = combination;
                    maxCount = count;
                }
            }
            return maxCombination;
        }

        public static void MarkClassified(List<Entity> subarray, List<ILAAttribute> combination) {
            // Mark all rows in the subarray as classified if they contain values from the combination.
            foreach (var row in subarray) {
                if (row.Attributes.All(a => combination.Contains(a))) {
                    row.IsClassified = true;
                }
            }
        }
    }
}
