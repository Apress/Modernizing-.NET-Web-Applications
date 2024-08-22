namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class Recording
    {
        public int Id { get; set; }

        public string AzureAssetId { get; set; }

        public string PublicLinkUrl { get; set; }

        public string Description { get; set; }

        public int CourseId { get; set; }

        public virtual Course Cours { get; set; }
    }
}
