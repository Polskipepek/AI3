namespace AI3.Entities {
    public class Rule {
        public List<ILAAttribute> Attributes { get; set; }
        public ILAAttribute DecisionAttribute { get; set; }

        public Rule() { }
        public Rule(List<ILAAttribute> attributes, ILAAttribute decisionAttribute) {
            Attributes = attributes;
            DecisionAttribute = decisionAttribute;
        }
    }
}
