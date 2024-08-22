using System;
using ReservationDemo.Domain.Services;

namespace ReservationDemo.App.Services;

public class CurrentCustomerProvider : ICurrentCustomerProvider
{
    // TODO: read from ClaimsIdentity
    public Guid CustomerId => Guid.Parse("897A1B7C-B7D1-44A9-A751-82AD0D0B5331");

    public Guid UserId => Guid.Parse("630F65BE-A38A-4051-BFE7-385B83DA30AC");
}