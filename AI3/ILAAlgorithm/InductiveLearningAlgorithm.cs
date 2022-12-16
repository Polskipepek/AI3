using AI3.Entities;

namespace AI3.ILAAlgorithm {
    public static class InductiveLearningAlgorithm {

        public static List<Rule> Learn(IEnumerable<Entity> data) {
            var ruleset = new List<Rule>();

            // Divide the data into subarrays
            var subarrays = new List<Entity>[data.Select(x => x.decitionAttribute).Distinct().Count()];
            for (int i = 0; i < subarrays.Length; i++) {
                subarrays[i] = new List<Entity>();
            }

            foreach (var row in data) {
                subarrays[row.decitionAttribute].Add(row);
            }

            // Repeat for each subarray
            foreach (var subarray in subarrays.OrderByDescending(x => x.First().decitionAttribute)) {
                var classifiedRows = new List<Entity>();
                var subarraysWithoutCurrent = subarrays.ToList();
                subarraysWithoutCurrent.Remove(subarray);

                // Continue looking for the most frequent combination of attributes until all rows are classified
                while (classifiedRows.Count < subarray.Count) {
                    var j = 1; // Initialize the number of attribute combinations to 1

                    // Find the most frequent combination of attributes with j attributes
                    var mostFrequent = subarray
                        .Except(classifiedRows)
                        .Where(g => subarraysWithoutCurrent.All(s => !s.Any(x => g.Attributes.All(c => x.Attributes.Contains(c)))))
                        .GroupBy(row => row.Attributes.Take(j))
                        .OrderByDescending(g => g.Count())
                        .First();

                    // If the most frequent combination of attributes is not found,
                    // increase the number of attributes in the combination and try again
                    while (!mostFrequent.Key.Any()) {
                        j++;
                        mostFrequent = subarray
                            .Except(classifiedRows)
                            .Where(g => subarrays.All(s => !s.Any(x => g.Attributes.All(c => x.Attributes.Contains(c)))))
                            .GroupBy(row => row.Attributes.Take(j))
                            .OrderByDescending(g => g.Count())
                            .First();
                    }

                    // Mark all rows that contain the most frequent combination of attributes as classified
                    classifiedRows.AddRange(subarray.Where(row => row.Attributes.Take(j).SequenceEqual(mostFrequent.Key)));

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
    }
}
