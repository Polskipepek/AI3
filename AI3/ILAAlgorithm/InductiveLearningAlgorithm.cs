using AI3.Entities;
using AI3.Extentions;

namespace AI3.ILAAlgorithm {
    public static class InductiveLearningAlgorithm {

        public static List<Rule> Learn(IEnumerable<Entity> data) {
            var ruleset = new List<Rule>();
            List<ILAAttribute> mostFrequentCombos = new();

            var subarrays = new List<Entity>[data.Select(x => x.decitionAttribute).Distinct().Count()];
            for (int i = 0; i < subarrays.Length; i++) {
                subarrays[i] = new List<Entity>();
            }

            foreach (var row in data) {
                subarrays[row.decitionAttribute].Add(row);
            }

            foreach (var subarray in subarrays.OrderByDescending(x => x.First().decitionAttribute)) {
                var subarraysWithoutCurrent = subarrays
                    .Where(x => x != subarray)
                    .SelectMany(x => x)
                    .SelectMany(x => x.Attributes)
                    .ToList();

                int j = 1;
                List<ILAAttribute> KOMBINACJA_MAKSYMALNA = new ();

                while (subarray.Any(x => !x.IsClassified)) {
                    for (int i = 0; i < subarray.First().Attributes.Count(); i++) {
                        if (i + j > subarray.First().Attributes.Count()) {
                            continue;
                        }

                        var currentCombination = subarray
                            .SelectMany(x => x.Attributes.Select(a => a.Name).Skip(i).Take(j))
                            .Distinct()
                            .ToList();

                        var correctValues = subarray
                           .Select(x => x.Attributes)
                           .SelectMany(x => x.Where(xx => currentCombination.Contains(xx.Name)))
                           .Where(x => !subarraysWithoutCurrent.Any(a => a.Value.Equals(x.Value)))
                           .GroupBy(x => x.Value)
                           .OrderByDescending(x => x.Count())
                           .ToList();

                        if (correctValues.Count == 0) {
                            throw new Exception("NIE MA KOMBINACJI.");
                        }

                        var maxValues = correctValues
                            .Where(g => g.Count() == correctValues.Max(a => a.Count()))
                            .SelectMany(x => x)
                            .DistinctBy(x => x.Name)
                            .ToList();
                        
                        Entity entity = null;

                        if (maxValues.Count > KOMBINACJA_MAKSYMALNA.Count) {
                            foreach (var combination in maxValues) {
                                entity = subarray.First(e => e.Attributes.Contains(combination));
                            }
                            KOMBINACJA_MAKSYMALNA = maxValues;
                        }

                        int a = 0;

                    }
                    //        var combinations = subarray
                    //.SelectMany(x => x.Attributes)
                    //.Select((x, i) => new { x, i })
                    //.GroupBy(x => x.i / j, x => x.x)
                    //.Select(x => x.ToList());


                    //var mostFrequent = subarray
                    //    .Where(g => subarraysWithoutCurrent.All(s => !s.Any(x => g.Attributes.All(c => x.Attributes.Contains(c)))))
                    //    .GroupBy(row => row.Attributes.Take(j))
                    //    .OrderByDescending(g => g.Count())
                    //    .First();

                    // If the most frequent combination of attributes is not found,
                    // increase the number of attributes in the combination and try again
                    // while (!mostFrequent.Key.Any())
                    //{
                    j++;
                    //    mostFrequent = subarray
                    //        .Where(g => subarraysWithoutCurrent.All(s => !s.Any(x => g.Attributes.All(c => x.Attributes.Contains(c)))))
                    //        .GroupBy(row => row.Attributes.Take(j))
                    //        .OrderByDescending(g => g.Count())
                    //        .First();
                    //}


                    // Create a new rule for the most frequent combination of attributes
                    //var rule = new Rule {
                    //  Attributes = mostFrequent.Key.ToList(),
                    //    DecisionAttribute = subarray.First().Attributes.Last()
                    //};

                    // Add the rule to the ruleset
                    // ruleset.Add(rule);
                    // }
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
