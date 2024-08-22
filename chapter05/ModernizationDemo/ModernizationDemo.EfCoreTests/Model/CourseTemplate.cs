using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.EfCoreTests.Model;

public partial class CourseTemplate
{
    [Key]
    public int Id { get; set; }

    public bool IsDeleted { get; set; }

    [StringLength(200)]
    public string? Name { get; set; }

    public string? Description { get; set; }

    [StringLength(200)]
    public string? RequiredKnowledge { get; set; }

    public int TypicalNumberOfDays { get; set; }

    public bool IsPrivate { get; set; }

    [StringLength(50)]
    public string? Code { get; set; }

    [StringLength(200)]
    public string? UrlName { get; set; }

    [StringLength(500)]
    public string? Keywords { get; set; }

    public int? PromotionOrder { get; set; }

    [InverseProperty("CourseTemplate")]
    public virtual ICollection<CourseTemplateRelation> CourseTemplateRelationCourseTemplates { get; set; } = new List<CourseTemplateRelation>();

    [InverseProperty("RelatedCourseTemplate")]
    public virtual ICollection<CourseTemplateRelation> CourseTemplateRelationRelatedCourseTemplates { get; set; } = new List<CourseTemplateRelation>();

    [InverseProperty("CourseTemplate")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    [InverseProperty("CourseTemplate")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ForeignKey("CourseTemplatesId")]
    [InverseProperty("CourseTemplates")]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
