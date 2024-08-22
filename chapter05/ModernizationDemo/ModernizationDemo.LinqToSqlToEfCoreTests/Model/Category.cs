using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? UrlName { get; set; }

    public string? Description { get; set; }

    public int? PromotionOrder { get; set; }

    public int MainCategoryId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual MainCategory MainCategory { get; set; } = null!;

    public virtual ICollection<CourseTemplate> CourseTemplates { get; set; } = new List<CourseTemplate>();
}
