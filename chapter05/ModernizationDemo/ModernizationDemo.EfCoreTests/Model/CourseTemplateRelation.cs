using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModernizationDemo.EfCoreTests.Model;

[Index("CourseTemplateId", Name = "IX_CourseTemplateRelations_CourseTemplateId")]
[Index("RelatedCourseTemplateId", Name = "IX_CourseTemplateRelations_RelatedCourseTemplateId")]
public partial class CourseTemplateRelation
{
    [Key]
    public int Id { get; set; }

    public int CourseTemplateId { get; set; }

    public int RelatedCourseTemplateId { get; set; }

    public int Type { get; set; }

    [ForeignKey("CourseTemplateId")]
    [InverseProperty("CourseTemplateRelationCourseTemplates")]
    public virtual CourseTemplate CourseTemplate { get; set; } = null!;

    [ForeignKey("RelatedCourseTemplateId")]
    [InverseProperty("CourseTemplateRelationRelatedCourseTemplates")]
    public virtual CourseTemplate RelatedCourseTemplate { get; set; } = null!;

    class EntityConfiguration : IEntityTypeConfiguration<CourseTemplateRelation>
    {
        public void Configure(EntityTypeBuilder<CourseTemplateRelation> entity)
        {
            entity.HasOne(d => d.RelatedCourseTemplate).WithMany(p => p.CourseTemplateRelationRelatedCourseTemplates).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
