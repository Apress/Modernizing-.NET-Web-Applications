namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class InvoiceLine
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public string Text { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal VATRate { get; set; }

        public decimal Units { get; set; }

        public string UnitName { get; set; }

        public decimal TotalAmount { get; set; }

        public virtual Invoice Invoice { get; set; }
    }
}
