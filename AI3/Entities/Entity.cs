namespace AI3.Entities {
    public class Entity {
        public IEnumerable<ILAAttribute> Attributes { get; set; }
        public int decisionAttribute { get; set; }
        public bool IsClassified { get; set; }
    }
}
