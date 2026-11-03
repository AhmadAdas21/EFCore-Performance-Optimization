
using API_with_DOCKER.DataBase;
using Microsoft.EntityFrameworkCore;

namespace API_with_DOCKER.DesignsPatterns.ChainOfResponsibility;

public class CustomerValidationHandler : OrderValidationHandler
{
    private readonly AppDbContext _context;

    public CustomerValidationHandler(AppDbContext context)
    {
        _context = context;
    }

    protected override async Task ValidateCurrentAsync( OrderValidationContext context,CancellationToken cancellationToken)
    {
        int customerId = context.Request.CustomerId;

        if (customerId <= 0)
        {
            throw new ArgumentException("Invalid customer ID.");
        }

        bool customerExists = await _context.Customers.AnyAsync(customer => customer.Id == customerId,cancellationToken);

        if (!customerExists)
        {
            throw new KeyNotFoundException("Customer not found.");
        }
    }
}
