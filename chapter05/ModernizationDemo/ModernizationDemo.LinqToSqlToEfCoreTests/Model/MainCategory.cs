using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class MainCategory
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? UrlName { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
