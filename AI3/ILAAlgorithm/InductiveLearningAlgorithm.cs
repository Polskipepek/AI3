using AI3.Entities;
using AI3.Extentions;

namespace AI3.ILAAlgorithm {
    public static class InductiveLearningAlgorithm {

        public static List<Rule> Learn(IEnumerable<Entity> data) {
            int maxFrequency = 0;
            var ruleset = new List<Rule>();
            List<ILAAttribute> mostFrequentCombos = new();

            var subarrays = new List<Entity>[data.Select(x => x.decitionAttribute).Distinct().Count()];
            for (int i = 0; i < subarrays.Length; i++) {
                subarrays[i] = new List<Entity>();
            }

            foreach (var row in data) {
                subarrays[row.decitionAttribute].Add(row);
            }

            // Repeat for each subarray
            foreach (var subarray in subarrays.OrderByDescending(x => x.First().decitionAttribute)) {
                var subarraysWithoutCurrent = subarrays.ToList();
                subarraysWithoutCurrent.Remove(subarray);
                int j = 1;

                for (int i = 0; i < subarray.First().Attributes.Count(); i++) {
                    if (i + j > subarray.First().Attributes.Count()) {
                        throw new Exception("out of bounds. :(");
                    }

                    var currentCombination = subarray
                        .SelectMany(x => x.Attributes.Select(a => a.Name).Skip(i).Take(j))
                        .Distinct()
                        .ToList();

                    var mostFrequentCombo1 = subarray
                       .Select(x => x.Attributes)
                       .Where(la => la.Any(a => currentCombination.Any(c => c.Equals(a.Name))));

                    var mostFrequentCombo2 = mostFrequentCombo1
                       .Where(g => subarraysWithoutCurrent.All(s => !s.Any(x => g.All(c => x.Attributes.Any(a => a.Value.Equals(c.Value))))))
                       .OrderByDescending(x => x)
                       .ToList();



                    var mostFrequent = subarray
                        .Where(g => subarraysWithoutCurrent.All(s => !s.Any(x => g.Attributes.All(c => x.Attributes.Contains(c)))))
                        .GroupBy(row => row.Attributes.Take(j))
                        .OrderByDescending(g => g.Count())
                        .First();

                    // If the most frequent combination of attributes is not found,
                    // increase the number of attributes in the combination and try again
                    while (!mostFrequent.Key.Any()) {
                        j++;
                        mostFrequent = subarray
                            .Where(g => subarraysWithoutCurrent.All(s => !s.Any(x => g.Attributes.All(c => x.Attributes.Contains(c)))))
                            .GroupBy(row => row.Attributes.Take(j))
                            .OrderByDescending(g => g.Count())
                            .First();
                    }


                    // Create a new rule for the most frequent combination of attributes
                    var rule = new Rule {
                        Attributes = mostFrequent.Key.ToList(),
                        DecisionAttribute = subarray.First().Attributes.Last()
                    };

                    // Add the rule to the ruleset
                    ruleset.Add(rule);
                }
            }

            return ruleset;
        }

        static List<ILAAttribute> FindMostFrequentCombo(List<List<Entity>> subarrays) {
            List<ILAAttribute> mostFrequentCombos = new();
            int maxFrequency = 0;

            foreach (var subarray in subarrays) {
                // Check all combinations of attributes from 1 to the number of attributes in each entity
                for (int i = 1; i <= subarray[0].Attributes.Count(); i++) {
                    var currentCombinations = subarray
                       .SelectMany(x => x.Attributes.Combinations(i))
                       .Distinct()
                       .ToList();

                    // Check the frequency of each combination in the current subarray
                    foreach (var currentCombination in currentCombinations) {
                        int frequency = subarray
                           .Count(x => x.Attributes.All(a => currentCombination.Contains(a)));

                        // Check if the combination does not exist in the other subarrays
                        bool existsInOtherSubarrays = subarrays
                           .Where(s => s != subarray)
                           .Any(s => s.Any(x => currentCombination.All(c => x.Attributes.Any(a => a.Value.Equals(c.Value)))));

                        if (frequency > maxFrequency && !existsInOtherSubarrays) {
                            maxFrequency = frequency;
                            mostFrequentCombos = new List<ILAAttribute>();
                            mostFrequentCombos.AddRange(currentCombination);
                        } else if (frequency == maxFrequency && !existsInOtherSubarrays) {
                            mostFrequentCombos.AddRange(currentCombination);
                        }
                    }
                }
            }

            return mostFrequentCombos;
        }
    }
}
