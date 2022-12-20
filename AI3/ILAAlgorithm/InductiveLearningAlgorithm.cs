using AI3.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI3.ILAAlgorithm {
    internal class InductiveLearningAlgorithm {
        public List<Rule> Learn(IEnumerable<Entity> data) {
            // Step 1: Divide the array (m examples) into n subarrays (n is the number of classes). 
            // One subarray for each possible value of the class attribute (for each class), set R as an empty set.
            var ruleset = new List<Rule>();
            var subarrays = new Dictionary<int, List<Entity>>();
            var combinations = CombinationGenerator.GetCombinations(data.First().Attributes.Select(x => x.Name));
            //var combinations = new Dictionary<int, List<List<string>>>();

            DivideArrayIntoSubarrays(data, subarrays);
            GetAttributesCombinations(data.First().Attributes.Select(x => x.Name));

            // Steps 2-8 are repeated for each subarray
            foreach (var subarray in subarrays.Values) {
                // Step 2: Initialize the number of attribute combinations j as j=1 (set j=1)
                int numAttributeCombinations = 1;
                var currentCombinations = combinations.Where(x => x.Count() == numAttributeCombinations);

                var otherSubarrays = subarrays.Where(x => !x.Value.Equals(subarray));
                // Step 3: For the considered subarray, divide the list of attributes into distinct combinations, 
                // each combination with j distinct attributes

                while (true) {
                    // Step 4: For each attribute combination, count the number of occurrences of the class attribute 
                    // value that appear under this same attribute combination in the unmarked rows of the considered subarray, 
                    // but which at the same time do not appear under this same attribute combination in other subarrays. 
                    // Call the first combination with the maximum number of occurrences as COMBINATION_MAXIMUM

                    if (!currentCombinations.Any()) {
                        break;
                    }

                    foreach (var combination in currentCombinations) {
                        var rowsThatArentClassified = subarray.Where(x => !x.IsClassified);
                        var rowsWithAttributesThatArentInOtherSubarrays = rowsThatArentClassified
                            .Where(x =>x );
                        //todo rowsWithAttributesThatArentInOtherSubarrays
                    }

                    // Step 5: If COMBINATION_MAXIMUM=0, increase j by 1 and go to step 3 (j=j+1)


                    // Step 6: Mark as classified all rows in the considered subarray where values from COMBINATION_MAXIMUM appear
                    //MarkClassified(subarray, maxCombination);

                    // Step 7: Add a rule to R, the left side of this rule contains the names of the attributes from COMBINATION_MAXIMUM 
                    // with their values, separated by the logical AND operator, and the right side contains the value of the decision attribute 
                    // associated with the subarray (class)
                    ruleset.Add(new Rule {
                        Attributes = null,
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

        private void GetAttributesCombinations(IEnumerable<string> columns) {
            var b = CombinationGenerator.GetCombinations(columns);
        }

        private static void DivideArrayIntoSubarrays(IEnumerable<Entity> data, Dictionary<int, List<Entity>> subarrays) {
            foreach (var row in data) {
                if (!subarrays.ContainsKey(row.decisionAttribute)) {
                    subarrays[row.decisionAttribute] = new List<Entity>();
                }
                subarrays[row.decisionAttribute].Add(row);
            }
        }
    }
}
