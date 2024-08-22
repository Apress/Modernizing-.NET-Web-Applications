using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.EfCoreTests.Model;

public partial class Location
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string? Name { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(200)]
    public string? City { get; set; }

    [Column("ZIP")]
    [StringLength(200)]
    public string? Zip { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string? HowToGetThere { get; set; }

    public int GeekcoreLocationId { get; set; }

    [InverseProperty("Location")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
