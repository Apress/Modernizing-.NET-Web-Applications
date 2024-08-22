using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("MainCategoryId", Name = "IX_Categories_MainCategoryId")]
public partial class Category
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string? Name { get; set; }

    [StringLength(200)]
    public string? UrlName { get; set; }

    public string? Description { get; set; }

    public int? PromotionOrder { get; set; }

    public int MainCategoryId { get; set; }

    public string? ImageUrl { get; set; }

    [ForeignKey("MainCategoryId")]
    [InverseProperty("Categories")]
    public virtual MainCategory MainCategory { get; set; } = null!;

    [ForeignKey("CategoriesId")]
    [InverseProperty("Categories")]
    public virtual ICollection<CourseTemplate> CourseTemplates { get; set; } = new List<CourseTemplate>();

    class EntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            entity.HasMany(d => d.CourseTemplates).WithMany(p => p.Categories)
                .UsingEntity<Dictionary<string, object>>(
                    "CategoryCourseTemplate",
                    r => r.HasOne<CourseTemplate>().WithMany().HasForeignKey("CourseTemplatesId"),
                    l => l.HasOne<Category>().WithMany().HasForeignKey("CategoriesId"),
                    j =>
                    {
                        j.HasKey("CategoriesId", "CourseTemplatesId");
                        j.ToTable("CategoryCourseTemplate");
                        j.HasIndex(new[] { "CourseTemplatesId" }, "IX_CategoryCourseTemplate_CourseTemplatesId");
                    });
        }
    }
}
