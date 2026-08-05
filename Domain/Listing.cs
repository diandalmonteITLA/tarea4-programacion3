namespace Domain
{
    public class Listing
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public required double Price { get; set; }
        public required string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
