using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class Location
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Zip { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string? HowToGetThere { get; set; }

    public int GeekcoreLocationId { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
