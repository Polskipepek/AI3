namespace AI3.Entities {
    public class Entity {
        public List<ILAAttribute> Attributes { get; set; } = new List<ILAAttribute>();
        public string DecisionAttribute { get; set; }
        public bool IsClassified { get; set; } = false;
    }
}
