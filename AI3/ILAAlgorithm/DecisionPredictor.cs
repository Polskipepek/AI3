using AI3.Entities;

namespace AI3.ILAAlgorithm {
    public class DecisionPredictor {
        private List<Rule> rules;

        public void SetRules(IEnumerable<Rule> rules) {
            this.rules = rules.OrderByDescending(r => r.Attributes.Count).ToList();
        }

        public string PredictDecision(Entity entity) {
            foreach (var rule in rules) {
                if (entity.Attributes.Count(a => rule.Attributes.Any(r => r.Name.Equals(a.Name) && r.Value.Equals(a.Value))) == rule.Attributes.Count) {
                    return rule.DecisionAttribute;
                }
            }
            return default;
        }
    }
}
