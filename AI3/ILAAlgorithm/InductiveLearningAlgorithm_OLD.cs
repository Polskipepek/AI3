using AI3.Entities;
using AI3.Extentions;

namespace AI3.ILAAlgorithm {
    public static class InductiveLearningAlgorithm_OLD {

        //public static List<Rule> Learn(IEnumerable<Entity> data) {
        //    // Step 1: Divide the array (m examples) into n subarrays (n is the number of classes). 
        //    // One subarray for each possible value of the class attribute (for each class), set R as an empty set.
        //    var ruleset = new List<Rule>();
        //    var subarrays = new Dictionary<int, List<Entity>>();

        //    foreach (var row in data) {
        //        if (!subarrays.ContainsKey(row.decisionAttribute)) {
        //            subarrays[row.decisionAttribute] = new List<Entity>();
        //        }
        //        subarrays[row.decisionAttribute].Add(row);
        //    }

        //    // Steps 2-8 are repeated for each subarray
        //    foreach (var subarray in subarrays.Values) {
        //        // Step 2: Initialize the number of attribute combinations j as j=1 (set j=1)
        //        int numAttributeCombinations = 1;

        //        // Step 3: For the considered subarray, divide the list of attributes into distinct combinations, 
        //        // each combination with j distinct attributes
        //        var attributeCombinations = GetAttributeCombinations(subarray, numAttributeCombinations);

        //        while (attributeCombinations.Any()) {
        //            // Step 4: For each attribute combination, count the number of occurrences of the class attribute 
        //            // value that appear under this same attribute combination in the unmarked rows of the considered subarray, 
        //            // but which at the same time do not appear under this same attribute combination in other subarrays. 
        //            // Call the first combination with the maximum number of occurrences as COMBINATION_MAXIMUM
        //            var maxCombination = GetMaxCombination(subarray, attributeCombinations, subarrays);

        //            // Step 5: If COMBINATION_MAXIMUM=0, increase j by 1 and go to step 3 (j=j+1)
        //            if (maxCombination == null) {
        //                numAttributeCombinations++;
        //                attributeCombinations = GetAttributeCombinations(subarray, numAttributeCombinations);
        //                continue;
        //            }

        //            // Step 6: Mark as classified all rows in the considered subarray where values from COMBINATION_MAXIMUM appear
        //            MarkClassified(subarray, maxCombination);

        //            // Step 7: Add a rule to R, the left side of this rule contains the names of the attributes from COMBINATION_MAXIMUM 
        //            // with their values, separated by the logical AND operator, and the right side contains the value of the decision attribute 
        //            // associated with the subarray (class)
        //            ruleset.Add(new Rule {
        //                Attributes = maxCombination,
        //                DecisionAttribute = subarray.First().decisionAttribute
        //            });

        //            // Step 8: If all rows in the considered subarray are marked as classified, then go to processing the next subarray (go to step 2)
        //            if (subarray.All(x => x.IsClassified)) {
        //                break;
        //            }

        //            // If there are still unmarked rows, go to step 4
        //            // If there are no more subarrays, return the current set of rules R
        //        }
        //    }
        //    return ruleset;
        //}

        //private static ILAAttribute GetMaxCombination(List<Entity> subarray, List<string> combinations, List<List<Entity>> otherSubarrays) {
        //    // Dictionary to store the count of each combination in the subarray
        //    Dictionary<ILAAttribute, int> combinationCounts = new Dictionary<ILAAttribute, int>();
        //    foreach (Entity entity in subarray) {
        //        bool allCombinationsPresent = true;
        //        foreach (string name in combinations) {
        //            if (!entity.Attributes.Select(x=>x.Name).Contains(name)) {
        //                allCombinationsPresent = false;
        //                break;
        //            }
        //        }

        //        if (allCombinationsPresent) {
        //            foreach (ILAAttribute attribute in entity.Attributes) {
        //                if (combinations.Contains(attribute.Name)) {
        //                    if (combinationCounts.ContainsKey(attribute)) {
        //                        combinationCounts[attribute]++;
        //                    } else {
        //                        combinationCounts[attribute] = 1;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    // Find the combination with the maximum count that does not exist in the other subarrays
        //    int maxCount = 0;
        //    ILAAttribute maxCombination = null;
        //    foreach (KeyValuePair<ILAAttribute, int> count in combinationCounts) {
        //        bool existsInOtherSubarrays = false;
        //        foreach (List<Entity> otherSubarray in otherSubarrays) {
        //            foreach (Entity otherEntity in otherSubarray) {
        //                if (otherEntity.Attributes.Contains(count.Key)) {
        //                    existsInOtherSubarrays = true;
        //                    break;
        //                }
        //            }
        //        }

        //        if (!existsInOtherSubarrays && count.Value > maxCount) {
        //            maxCount = count.Value;
        //            maxCombination = count.Key;
        //        }
        //    }

        //    return maxCombination;
        //}



        //private static List<List<string>> GetAttributeCombinations(List<Entity> subarray, int numAttributeCombinations) {
        //    var attributeCombinations = new List<List<string>>();
        //    foreach (var entity in subarray) {
        //        // Generate all combinations of `numAttributeCombinations` attributes for each entity
        //        var entityAttributeCombinations = GenerateAttributeCombinations(entity.Attributes.ToList(), numAttributeCombinations);
        //        attributeCombinations.AddRange(entityAttributeCombinations);
        //    }
        //    return attributeCombinations;
        //}

        //private static List<List<string>> GenerateAttributeCombinations(List<ILAAttribute> attributes, int numAttributes) {
        //    var combinations = new List<List<string>>();
        //    // Generate all combinations of `numAttributes` attributes using recursion
        //    GenerateAttributeCombinationsRecursive(attributes, numAttributes, new List<string>(), combinations);
        //    return combinations;
        //}

        //private static void GenerateAttributeCombinationsRecursive(List<ILAAttribute> attributes, int numAttributes, List<string> currentCombination, List<List<string>> combinations) {
        //    if (currentCombination.Count == numAttributes) {
        //        combinations.Add(new List<string>(currentCombination));
        //        return;
        //    }
        //    for (int i = 0; i < attributes.Count; i++) {
        //        currentCombination.Add(attributes[i].Name);
        //        GenerateAttributeCombinationsRecursive(attributes, numAttributes, currentCombination, combinations);
        //        currentCombination.RemoveAt(currentCombination.Count - 1);
        //    }
        //}

        //private static List<ILAAttribute> GetMaxCombination(List<Entity> subarray, List<List<ILAAttribute>> combinations) {
        //    // Find the combination with the maximum number of occurrences in the subarray.
        //    var maxCombination = new List<ILAAttribute>();
        //    var maxCount = 0;
        //    foreach (var combination in combinations) {
        //        var count = 0;
        //        foreach (var row in subarray) {
        //            if (row.Attributes.All(a => combination.Contains(a))) {
        //                count++;
        //            }
        //        }
        //        if (count > maxCount) {
        //            maxCombination = combination;
        //            maxCount = count;
        //        }
        //    }
        //    return maxCombination;
        //}

        private static void MarkClassified(List<Entity> subarray, List<ILAAttribute> combination) {
            // Mark all rows in the subarray as classified if they contain values from the combination.
            foreach (var row in subarray) {
                if (row.Attributes.Any(a => combination.Contains(a))) {
                    row.IsClassified = true;
                }
            }
        }
    }
}
