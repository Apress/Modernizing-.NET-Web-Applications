using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.EfCoreTests.Model;

public partial class MainCategory
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string? Name { get; set; }

    [StringLength(200)]
    public string? UrlName { get; set; }

    [InverseProperty("MainCategory")]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
