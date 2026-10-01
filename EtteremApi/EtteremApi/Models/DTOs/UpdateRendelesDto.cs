namespace EtteremApi.Models.DTOs
{
    public class UpdateRendelesDto
    {
        public string Dish { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int VendegId { get; set; }
    }
}
