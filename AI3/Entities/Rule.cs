namespace AI3.Entities {
    public class Rule {
        public List<ILAAttribute> Attributes { get; set; }
        public int DecisionAttribute { get; set; }

        public Rule() { }
        public Rule(List<ILAAttribute> attributes, int decisionAttribute) {
            Attributes = attributes;
            DecisionAttribute = decisionAttribute;
        }
    }
}
