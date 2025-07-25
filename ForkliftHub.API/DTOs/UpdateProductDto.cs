namespace ForkliftHub.Api.DTOs
{
    public class UpdateProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public double? LiftingHeight { get; set; }
        public double? ClosedHeight { get; set; }
        public string? ImageUrl { get; set; }

        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int ProductTypeId { get; set; }
        public int EngineId { get; set; }
        public int MastTypeId { get; set; }
        public int MachineModelId { get; set; }
    }
}
