using AI3.Entities;
using System.Collections;

namespace AI3.ILAAlgorithm {
    internal class InductiveLearningAlgorithm {
        public List<Rule> Learn(IEnumerable<Entity> data) {
            var ruleset = new List<Rule>();
            var subarrays = new Dictionary<string, List<Entity>>();

            DivideArrayIntoSubarrays(data, subarrays);
            var pairs = new Hashtable();
            foreach (var subarray in subarrays.Values) {
                int maxCount = 0;
                List<ILAAttribute> maxCombination = new();
                int numAttributeCombinations = 1;

                var otherEntities = subarrays.Where(x => !x.Value.Equals(subarray)).SelectMany(x => x.Value);
                int prevNumOfAttributes = 0;
                var currentCombinations = GetCombinations(subarray, numAttributeCombinations);

                while (!subarray.All(x => x.IsClassified)) {
                    if (prevNumOfAttributes != numAttributeCombinations) {
                        currentCombinations = GetCombinations(subarray, numAttributeCombinations);
                        prevNumOfAttributes = numAttributeCombinations;
                    }

                    if (!currentCombinations.Any()) break;

                    foreach (var combination in currentCombinations) {
                        pairs.Clear();

                        var entities = subarray
                            .Where(e => !e.IsClassified)
                            .Where(entity =>
                                !otherEntities.Any(otherEntity => combination.All(c => otherEntity.Attributes.Any(a =>
                                    a.Name.Equals(c)
                                    && a.Value.Equals(entity.Attributes.First(at => at.Name.Equals(c)).Value)))));

                        GetMaxCombinationFromEntities(pairs, ref maxCount, ref maxCombination, combination, entities);
                    }

                    if (!maxCombination.Any()) {
                        numAttributeCombinations++;
                    } else {
                        MarkClassified(subarray, maxCombination);
                        CreateRule(ruleset, subarray, maxCombination);
                        maxCombination = new();
                        maxCount = 0;
                    }
                }
            }
            return ruleset;
        }

        private static IEnumerable<IEnumerable<string>> GetCombinations(List<Entity> subarray, int numAttributeCombinations) {
            return CombinationGenerator.GetCombinations(subarray.First().Attributes.Select(x => x.Name)).Where(x => x.Count() == numAttributeCombinations);
        }

        private static void CreateRule(List<Rule> ruleset, List<Entity> subarray, List<ILAAttribute> maxCombination) {
            ruleset.Add(new Rule {
                Attributes = maxCombination,
                DecisionAttribute = subarray.First(e => e.Attributes.All(a => !maxCombination.Any(c => c.Name.Equals(a.Name) && c.Value.Equals(a.Name)))).DecisionAttribute
            });
        }
        private static void GetMaxCombinationFromEntities(Hashtable pairs, ref int maxCount, ref List<ILAAttribute> maxCombination, IEnumerable<string> combination, IEnumerable<Entity> entities) {
            foreach (var entity in entities) {
                var key = combination.GetHashCode();
                if (!pairs.ContainsKey(key)) {
                    pairs[key] = (1, combination.Select(c => new ILAAttribute { Name = c, Value = entity.Attributes.First(a => a.Name.Equals(c)).Value }).ToList());
                } else {
                    var (tcount, tattributesCombination) = (ValueTuple<int, List<ILAAttribute>>)pairs[key];
                    pairs[key] = (tcount + 1, tattributesCombination);
                }

                var (count, attributesCombination) = (ValueTuple<int, List<ILAAttribute>>)pairs[key];
                if (count > maxCount && entities.Any()) {
                    maxCount = count;
                    maxCombination = attributesCombination;
                }
            }
        }

        //private static void GetMaxCombinationFromEntities(Dictionary<IEnumerable<ILAAttribute>, int> pairs, ref int maxCount, ref List<ILAAttribute> maxCombination, IEnumerable<string> combination, IEnumerable<Entity> entities) {
        //    foreach (var entity in entities) {
        //        foreach (var column in combination) {
        //            var pair = pairs.FirstOrDefault(a => a.Key.Select(x => x.Name).Contains(column) && a.Key.Select(x => x.Value).Contains(entity.Attributes.FirstOrDefault(a => a.Name.Equals(column))?.Value));
        //            if (pair.Key == null) {
        //                pairs.Add(combination.Select(c => new ILAAttribute() { Name = c, Value = entity.Attributes.First(a => a.Name.Equals(c)).Value }), 1);
        //            } else {
        //                pairs[pair.Key]++;
        //            }
        //        }

        //        var bestLocalCombination = pairs.OrderByDescending(x => x.Value).First();

        //        if (bestLocalCombination.Value > maxCount && entities.Any()) {
        //            maxCount = bestLocalCombination.Value;
        //            maxCombination = combination.Select(name => new ILAAttribute { Name = name, Value = bestLocalCombination.Key.First(a => a.Name.Equals(name)).Value }).ToList();
        //        }
        //    }
        //}

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
