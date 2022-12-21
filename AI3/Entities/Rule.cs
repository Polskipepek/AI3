namespace AI3.Entities {
    public class Rule {
        public List<ILAAttribute> Attributes { get; set; }
        public string DecisionAttribute { get; set; }

        public Rule() { }
        public Rule(List<ILAAttribute> attributes, string decisionAttribute) {
            Attributes = attributes;
            DecisionAttribute = decisionAttribute;
        }
    }
}
