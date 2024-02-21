using System.ComponentModel.DataAnnotations;

namespace Currency_Exchange.Models
{
    public class Currency
    {
        public int? Id { get; set; } = null;
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Code { get; set; } = string.Empty;
        public decimal? ExchangeRate { get; set; } = null;
        public int? CrUserId { get; set; } = null;
        public DateTime? CrDateTime { get; set; } = null;
        public int? MdUserId { get; set; } = null;
        public DateTime? MdDateTime { get; set; } = null;
    }
}
