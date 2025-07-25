namespace ForkliftHub.Api.DTOs
{
    public class ProductFilterDto
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public int? EngineId { get; set; }
        public int? MastTypeId { get; set; }
        public string? SortOrder { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
