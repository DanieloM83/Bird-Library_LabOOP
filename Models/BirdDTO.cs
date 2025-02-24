namespace BirdLab.Models {
    public enum Species
    {
        Cardinal,
        Sparrow,
        Eagle,
        Owl,
        Parrot,
        Crow,
        Penguin
    }

    public record BirdDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Species Species { get; set; }
        public string Info { get; set; }

        public override string ToString() => Name;
    }
}