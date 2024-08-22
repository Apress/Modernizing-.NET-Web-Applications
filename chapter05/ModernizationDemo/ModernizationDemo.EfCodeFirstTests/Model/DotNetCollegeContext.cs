using System.Data.Entity;

namespace ModernizationDemo.EfCodeFirstTests.Model
{
    public partial class DotNetCollegeContext : DbContext
    {
        public DotNetCollegeContext()
            : base("name=DotNetCollegeContext")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountTransaction> AccountTransactions { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<AttendeeRegistration> AttendeeRegistrations { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CourseDate> CourseDates { get; set; }
        public virtual DbSet<CourseReminder> CourseReminders { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseTemplateRelation> CourseTemplateRelations { get; set; }
        public virtual DbSet<CourseTemplate> CourseTemplates { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DiscountCoupon> DiscountCoupons { get; set; }
        public virtual DbSet<InvoiceLine> InvoiceLines { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Lector> Lectors { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<MainCategory> MainCategories { get; set; }
        public virtual DbSet<OrderAvailableDate> OrderAvailableDates { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<PrivateCourseRequest> PrivateCourseRequests { get; set; }
        public virtual DbSet<Recording> Recordings { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasMany(e => e.Lectors)
                .WithOptional(e => e.Account)
                .HasForeignKey(e => e.CommissionAccountId);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.Customers)
                .WithRequired(e => e.Address)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.Invoices)
                .WithRequired(e => e.Address)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.Orders)
                .WithOptional(e => e.Address)
                .HasForeignKey(e => e.BillingAddressId);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.CourseTemplates)
                .WithMany(e => e.Categories)
                .Map(m => m.ToTable("CategoryCourseTemplate").MapLeftKey("CategoriesId").MapRightKey("CourseTemplatesId"));

            modelBuilder.Entity<Course>()
                .HasMany(e => e.AccountTransactions)
                .WithOptional(e => e.Cours)
                .HasForeignKey(e => e.CourseId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Attachments)
                .WithRequired(e => e.Cours)
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.CourseDates)
                .WithRequired(e => e.Cours)
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.CourseReminders)
                .WithRequired(e => e.Cours)
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.DiscountCoupons)
                .WithOptional(e => e.Cours)
                .HasForeignKey(e => e.RestrictToCourseId);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Cours)
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Recordings)
                .WithRequired(e => e.Cours)
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Lectors)
                .WithMany(e => e.Courses)
                .Map(m => m.ToTable("CourseLector").MapLeftKey("CoursesId").MapRightKey("LectorsId"));

            modelBuilder.Entity<CourseTemplate>()
                .HasMany(e => e.CourseTemplateRelations)
                .WithRequired(e => e.CourseTemplate)
                .HasForeignKey(e => e.CourseTemplateId);

            modelBuilder.Entity<CourseTemplate>()
                .HasMany(e => e.CourseTemplateRelations1)
                .WithRequired(e => e.CourseTemplate1)
                .HasForeignKey(e => e.RelatedCourseTemplateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.AccountTransactions)
                .WithOptional(e => e.Invoice)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.Orders)
                .WithOptional(e => e.Invoice)
                .HasForeignKey(e => e.FinalInvoiceId);

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.Orders1)
                .WithOptional(e => e.Invoice1)
                .HasForeignKey(e => e.ProformaInvoiceId);

            modelBuilder.Entity<Lector>()
                .HasMany(e => e.Accounts)
                .WithOptional(e => e.Lector)
                .HasForeignKey(e => e.LectorId);

            modelBuilder.Entity<Lector>()
                .HasMany(e => e.PrivateCourseRequests)
                .WithMany(e => e.Lectors)
                .Map(m => m.ToTable("LectorPrivateCourseRequest").MapLeftKey("LectorsId").MapRightKey("PrivateCourseRequestsId"));

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Invoices)
                .WithRequired(e => e.Supplier)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Lectors)
                .WithRequired(e => e.Supplier)
                .HasForeignKey(e => e.DefaultSupplierId)
                .WillCascadeOnDelete(false);
        }
    }
}
