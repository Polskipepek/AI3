namespace AI3.Entities {
    public class ILAAttribute {
        public string Name { get; set; }
        public object Value { get; set; }

        public ILAAttribute() { }
        public ILAAttribute(string name, object value) {
            Name = name;
            Value = value;
        }
        override public string ToString() {
            return $"Name: {Name} \tValue: {Value}";
        }
    }
}
