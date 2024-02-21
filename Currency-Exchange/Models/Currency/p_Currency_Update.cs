namespace Currency_Exchange.Models
{
    public class p_Currency_Update
    {
        public int? p_Id { get; set; } = null;
        public string p_Name { get; set; } = string.Empty;
        public string p_Code { get; set; } = string.Empty;
        public decimal? p_ExchangeRate { get; set; } = null;
        public int? p_MdUserId { get; set; } = null;
        public DateTime? p_MdDateTime { get; set; } = null;
    }
}
