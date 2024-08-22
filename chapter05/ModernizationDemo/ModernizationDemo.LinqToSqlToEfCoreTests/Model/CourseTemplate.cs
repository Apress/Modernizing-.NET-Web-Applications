using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class CourseTemplate
{
    public int Id { get; set; }

    public bool IsDeleted { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? RequiredKnowledge { get; set; }

    public int TypicalNumberOfDays { get; set; }

    public bool IsPrivate { get; set; }

    public string? Code { get; set; }

    public string? UrlName { get; set; }

    public string? Keywords { get; set; }

    public int? PromotionOrder { get; set; }

    public virtual ICollection<CourseTemplateRelation> CourseTemplateRelationCourseTemplates { get; set; } = new List<CourseTemplateRelation>();

    public virtual ICollection<CourseTemplateRelation> CourseTemplateRelationRelatedCourseTemplates { get; set; } = new List<CourseTemplateRelation>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
