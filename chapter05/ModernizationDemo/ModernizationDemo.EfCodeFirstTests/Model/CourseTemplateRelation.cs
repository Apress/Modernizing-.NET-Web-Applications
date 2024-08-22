namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class CourseTemplateRelation
    {
        public int Id { get; set; }

        public int CourseTemplateId { get; set; }

        public int RelatedCourseTemplateId { get; set; }

        public int Type { get; set; }

        public virtual CourseTemplate CourseTemplate { get; set; }

        public virtual CourseTemplate CourseTemplate1 { get; set; }
    }
}
