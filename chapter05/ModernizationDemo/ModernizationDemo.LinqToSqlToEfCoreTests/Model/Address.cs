using System;
using System.Collections.Generic;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class Address
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? Zip { get; set; }

    public string? Ic { get; set; }

    public string? Dic { get; set; }

    public int? CountryId { get; set; }

    public string? CompanyRegistration { get; set; }

    public string? BankAccount { get; set; }

    public string? Iban { get; set; }

    public string? Swift { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
