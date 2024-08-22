using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Czech_CI_AS");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasIndex(e => e.LectorId, "IX_Accounts_LectorId");

            entity.HasIndex(e => e.SupplierId, "IX_Accounts_SupplierId");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Lector).WithMany(p => p.Accounts).HasForeignKey(d => d.LectorId);

            entity.HasOne(d => d.Supplier).WithMany(p => p.Accounts).HasForeignKey(d => d.SupplierId);
        });

        modelBuilder.Entity<AccountTransaction>(entity =>
        {
            entity.HasIndex(e => e.AccountId, "IX_AccountTransactions_AccountId");

            entity.HasIndex(e => e.CourseId, "IX_AccountTransactions_CourseId");

            entity.HasIndex(e => e.InvoiceId, "IX_AccountTransactions_InvoiceId");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomAccountTransactionAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("CustomAccountTransaction_Amount");
            entity.Property(e => e.CustomAccountTransactionCreatedDate).HasColumnName("CustomAccountTransaction_CreatedDate");
            entity.Property(e => e.CustomAccountTransactionPaidDate).HasColumnName("CustomAccountTransaction_PaidDate");
            entity.Property(e => e.InvoiceUrl).HasMaxLength(200);
            entity.Property(e => e.LectorInvoiceUrl).HasMaxLength(200);

            entity.HasOne(d => d.Account).WithMany(p => p.AccountTransactions).HasForeignKey(d => d.AccountId);

            entity.HasOne(d => d.Course).WithMany(p => p.AccountTransactions)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Invoice).WithMany(p => p.AccountTransactions)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasIndex(e => e.CountryId, "IX_Addresses_CountryId");

            entity.Property(e => e.BankAccount).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(200);
            entity.Property(e => e.CompanyRegistration).HasMaxLength(200);
            entity.Property(e => e.Dic)
                .HasMaxLength(20)
                .HasColumnName("DIC");
            entity.Property(e => e.Iban).HasMaxLength(50);
            entity.Property(e => e.Ic)
                .HasMaxLength(20)
                .HasColumnName("IC");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Street).HasMaxLength(200);
            entity.Property(e => e.Swift).HasMaxLength(50);
            entity.Property(e => e.Zip)
                .HasMaxLength(20)
                .HasColumnName("ZIP");

            entity.HasOne(d => d.Country).WithMany(p => p.Addresses).HasForeignKey(d => d.CountryId);
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasIndex(e => e.CourseId, "IX_Attachments_CourseId");

            entity.Property(e => e.FileName).HasMaxLength(200);
            entity.Property(e => e.Url).HasMaxLength(200);

            entity.HasOne(d => d.Course).WithMany(p => p.Attachments).HasForeignKey(d => d.CourseId);
        });

        modelBuilder.Entity<AttendeeRegistration>(entity =>
        {
            entity.HasIndex(e => e.OrderId, "IX_AttendeeRegistrations_OrderId");

            entity.HasIndex(e => e.UserId, "IX_AttendeeRegistrations_UserId");

            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FirstName).HasMaxLength(200);
            entity.Property(e => e.LastName).HasMaxLength(200);

            entity.HasOne(d => d.Order).WithMany(p => p.AttendeeRegistrations).HasForeignKey(d => d.OrderId);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasIndex(e => e.MainCategoryId, "IX_Categories_MainCategoryId");

            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.UrlName).HasMaxLength(200);

            entity.HasOne(d => d.MainCategory).WithMany(p => p.Categories).HasForeignKey(d => d.MainCategoryId);

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
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasIndex(e => e.CourseTemplateId, "IX_Courses_CourseTemplateId");

            entity.HasIndex(e => e.CustomerId, "IX_Courses_CustomerId");

            entity.HasIndex(e => e.LocationId, "IX_Courses_LocationId");

            entity.HasIndex(e => e.SupplierId, "IX_Courses_SupplierId");

            entity.Property(e => e.CourseSubtitle).HasMaxLength(200);
            entity.Property(e => e.Margin).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PricePerDay).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CourseTemplate).WithMany(p => p.Courses).HasForeignKey(d => d.CourseTemplateId);

            entity.HasOne(d => d.Customer).WithMany(p => p.Courses).HasForeignKey(d => d.CustomerId);

            entity.HasOne(d => d.Location).WithMany(p => p.Courses).HasForeignKey(d => d.LocationId);

            entity.HasOne(d => d.Supplier).WithMany(p => p.Courses).HasForeignKey(d => d.SupplierId);

            entity.HasMany(d => d.Lectors).WithMany(p => p.Courses)
                .UsingEntity<Dictionary<string, object>>(
                    "CourseLector",
                    r => r.HasOne<Lector>().WithMany().HasForeignKey("LectorsId"),
                    l => l.HasOne<Course>().WithMany().HasForeignKey("CoursesId"),
                    j =>
                    {
                        j.HasKey("CoursesId", "LectorsId");
                        j.ToTable("CourseLector");
                        j.HasIndex(new[] { "LectorsId" }, "IX_CourseLector_LectorsId");
                    });
        });

        modelBuilder.Entity<CourseDate>(entity =>
        {
            entity.HasIndex(e => e.CourseId, "IX_CourseDates_CourseId");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseDates).HasForeignKey(d => d.CourseId);
        });

        modelBuilder.Entity<CourseReminder>(entity =>
        {
            entity.HasIndex(e => e.AppUserId, "IX_CourseReminders_AppUserId");

            entity.HasIndex(e => e.CourseId, "IX_CourseReminders_CourseId");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseReminders).HasForeignKey(d => d.CourseId);
        });

        modelBuilder.Entity<CourseTemplate>(entity =>
        {
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Keywords).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.RequiredKnowledge).HasMaxLength(200);
            entity.Property(e => e.UrlName).HasMaxLength(200);
        });

        modelBuilder.Entity<CourseTemplateRelation>(entity =>
        {
            entity.HasIndex(e => e.CourseTemplateId, "IX_CourseTemplateRelations_CourseTemplateId");

            entity.HasIndex(e => e.RelatedCourseTemplateId, "IX_CourseTemplateRelations_RelatedCourseTemplateId");

            entity.HasOne(d => d.CourseTemplate).WithMany(p => p.CourseTemplateRelationCourseTemplates).HasForeignKey(d => d.CourseTemplateId);

            entity.HasOne(d => d.RelatedCourseTemplate).WithMany(p => p.CourseTemplateRelationRelatedCourseTemplates)
                .HasForeignKey(d => d.RelatedCourseTemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(e => e.AddressId, "IX_Customers_AddressId");

            entity.HasOne(d => d.Address).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DiscountCoupon>(entity =>
        {
            entity.HasIndex(e => e.RestrictToCourseId, "IX_DiscountCoupons_RestrictToCourseId");

            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Percent).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.RestrictToCourse).WithMany(p => p.DiscountCoupons).HasForeignKey(d => d.RestrictToCourseId);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasIndex(e => e.AddressId, "IX_Invoices_AddressId");

            entity.HasIndex(e => e.CustomerId, "IX_Invoices_CustomerId");

            entity.HasIndex(e => e.SupplierId, "IX_Invoices_SupplierId");

            entity.Property(e => e.CorrectedInvoiceNumber).HasMaxLength(20);
            entity.Property(e => e.FileUrl).HasMaxLength(200);
            entity.Property(e => e.Number).HasMaxLength(20);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalAmountWithoutVat)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalAmountWithoutVAT");

            entity.HasOne(d => d.Address).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices).HasForeignKey(d => d.CustomerId);

            entity.HasOne(d => d.Supplier).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<InvoiceLine>(entity =>
        {
            entity.HasIndex(e => e.InvoiceId, "IX_InvoiceLines_InvoiceId");

            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Units).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Vatrate)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("VATRate");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceLines).HasForeignKey(d => d.InvoiceId);
        });

        modelBuilder.Entity<Lector>(entity =>
        {
            entity.HasIndex(e => e.CommissionAccountId, "IX_Lectors_CommissionAccountId");

            entity.HasIndex(e => e.DefaultSupplierId, "IX_Lectors_DefaultSupplierId");

            entity.HasIndex(e => e.UserId, "IX_Lectors_UserId");

            entity.Property(e => e.AvatarUrl).HasMaxLength(200);
            entity.Property(e => e.Blog).HasMaxLength(200);
            entity.Property(e => e.CommissionPercent).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CommissionPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DefaultSupplierId).HasDefaultValue(4);
            entity.Property(e => e.LinkedIn).HasMaxLength(200);
            entity.Property(e => e.Twitter).HasMaxLength(200);
            entity.Property(e => e.UrlName).HasMaxLength(200);
            entity.Property(e => e.Website).HasMaxLength(200);

            entity.HasOne(d => d.CommissionAccount).WithMany(p => p.Lectors).HasForeignKey(d => d.CommissionAccountId);

            entity.HasOne(d => d.DefaultSupplier).WithMany(p => p.Lectors)
                .HasForeignKey(d => d.DefaultSupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasMany(d => d.PrivateCourseRequests).WithMany(p => p.Lectors)
                .UsingEntity<Dictionary<string, object>>(
                    "LectorPrivateCourseRequest",
                    r => r.HasOne<PrivateCourseRequest>().WithMany().HasForeignKey("PrivateCourseRequestsId"),
                    l => l.HasOne<Lector>().WithMany()
                        .HasForeignKey("LectorsId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("LectorsId", "PrivateCourseRequestsId");
                        j.ToTable("LectorPrivateCourseRequest");
                        j.HasIndex(new[] { "PrivateCourseRequestsId" }, "IX_LectorPrivateCourseRequest_PrivateCourseRequestsId");
                    });
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Zip)
                .HasMaxLength(200)
                .HasColumnName("ZIP");
        });

        modelBuilder.Entity<MainCategory>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.UrlName).HasMaxLength(200);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.BillingAddressId, "IX_Orders_BillingAddressId");

            entity.HasIndex(e => e.CourseId, "IX_Orders_CourseId");

            entity.HasIndex(e => e.CourseTemplateId, "IX_Orders_CourseTemplateId");

            entity.HasIndex(e => e.DiscountCouponId, "IX_Orders_DiscountCouponId");

            entity.HasIndex(e => e.FinalInvoiceId, "IX_Orders_FinalInvoiceId");

            entity.HasIndex(e => e.ProformaInvoiceId, "IX_Orders_ProformaInvoiceId");

            entity.HasIndex(e => e.UserId, "IX_Orders_UserId");

            entity.Property(e => e.BraintreeTransactionId).HasMaxLength(200);
            entity.Property(e => e.Currency).HasMaxLength(20);
            entity.Property(e => e.DiscountPercent).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HasVat).HasColumnName("HasVAT");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Vatrate)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("VATRate");

            entity.HasOne(d => d.BillingAddress).WithMany(p => p.Orders).HasForeignKey(d => d.BillingAddressId);

            entity.HasOne(d => d.Course).WithMany(p => p.Orders).HasForeignKey(d => d.CourseId);

            entity.HasOne(d => d.CourseTemplate).WithMany(p => p.Orders).HasForeignKey(d => d.CourseTemplateId);

            entity.HasOne(d => d.DiscountCoupon).WithMany(p => p.Orders).HasForeignKey(d => d.DiscountCouponId);

            entity.HasOne(d => d.FinalInvoice).WithMany(p => p.OrderFinalInvoices).HasForeignKey(d => d.FinalInvoiceId);

            entity.HasOne(d => d.ProformaInvoice).WithMany(p => p.OrderProformaInvoices).HasForeignKey(d => d.ProformaInvoiceId);
        });

        modelBuilder.Entity<OrderAvailableDate>(entity =>
        {
            entity.HasIndex(e => e.OrderId, "IX_OrderAvailableDates_OrderId");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderAvailableDates).HasForeignKey(d => d.OrderId);
        });

        modelBuilder.Entity<PrivateCourseRequest>(entity =>
        {
            entity.HasIndex(e => e.AppUserId, "IX_PrivateCourseRequests_AppUserId");

            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CompanyName).HasMaxLength(100);
            entity.Property(e => e.GrantFinancing)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0),(0)))");
        });

        modelBuilder.Entity<Recording>(entity =>
        {
            entity.HasIndex(e => e.CourseId, "IX_Recordings_CourseId");

            entity.HasOne(d => d.Course).WithMany(p => p.Recordings).HasForeignKey(d => d.CourseId);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasIndex(e => e.AddressId, "IX_Suppliers_AddressId");

            entity.Property(e => e.Currency).HasMaxLength(20);
            entity.Property(e => e.IsVatpayer).HasColumnName("IsVATPayer");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Vatrate)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("VATRate");

            entity.HasOne(d => d.Address).WithMany(p => p.Suppliers).HasForeignKey(d => d.AddressId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
