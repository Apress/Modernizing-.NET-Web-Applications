using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class CourseTemplateRelation
{
    public int Id { get; set; }

    public int CourseTemplateId { get; set; }

    public int RelatedCourseTemplateId { get; set; }

    public int Type { get; set; }

    public virtual CourseTemplate CourseTemplate { get; set; } = null!;

    public virtual CourseTemplate RelatedCourseTemplate { get; set; } = null!;
}
