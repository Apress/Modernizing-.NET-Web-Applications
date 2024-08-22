using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.EfCoreTests.Model;

public partial class DotNetCollegeContext : DbContext
{
    public DotNetCollegeContext()
    {
    }

    public DotNetCollegeContext(DbContextOptions<DotNetCollegeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountTransaction> AccountTransactions { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<AttendeeRegistration> AttendeeRegistrations { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseDate> CourseDates { get; set; }

    public virtual DbSet<CourseReminder> CourseReminders { get; set; }

    public virtual DbSet<CourseTemplate> CourseTemplates { get; set; }

    public virtual DbSet<CourseTemplateRelation> CourseTemplateRelations { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DiscountCoupon> DiscountCoupons { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceLine> InvoiceLines { get; set; }

    public virtual DbSet<Lector> Lectors { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<MainCategory> MainCategories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderAvailableDate> OrderAvailableDates { get; set; }

    public virtual DbSet<PrivateCourseRequest> PrivateCourseRequests { get; set; }

    public virtual DbSet<Recording> Recordings { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Czech_CI_AS");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DotNetCollegeContext).Assembly);

        OnModelCreatingPartial(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Add(_ => new KeyNamingConvention());

        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=EFTestsNew;Integrated Security=True;TrustServerCertificate=True");
        }

        base.OnConfiguring(optionsBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
