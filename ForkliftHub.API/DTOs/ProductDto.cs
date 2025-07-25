namespace ForkliftHub.Api.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public double? LiftingHeight { get; set; }
        public double? ClosedHeight { get; set; }
        public string? ImageUrl { get; set; }

        public string BrandName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string ProductTypeName { get; set; } = null!;
        public string EngineType { get; set; } = null!;
        public string MastTypeName { get; set; } = null!;
        public string MachineModelName { get; set; } = null!;
    }
}
