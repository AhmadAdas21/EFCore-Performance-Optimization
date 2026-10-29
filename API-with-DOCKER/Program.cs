
using API_with_DOCKER.DataBase;
using API_with_DOCKER.interfaces;
using API_with_DOCKER.services;
using API_with_DOCKER.DesignsPatterns.Pricing;
using API_with_DOCKER.DesignsPatterns.Factory;
using Microsoft.EntityFrameworkCore;
using API_with_DOCKER.DesignsPatterns.Decorater;
using Microsoft.Extensions.Caching.Distributed;
using API_with_DOCKER.DesignsPatterns.ChainOfResponsibility;
using API_with_DOCKER.DesignsPatterns.Adapter;
using API_with_DOCKER.ThirdPartyPayment;
using API_with_DOCKER.DesignsPatterns.Results;
var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Product and Order Services

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =builder.Configuration["Redis:ConnectionString"]?? throw new InvalidOperationException("Redis connection string is missing");

    options.InstanceName = "APIWithDocker:";
});

builder.Services.AddScoped<ProductServices>();

builder.Services.AddScoped<IProductServices>(sp =>
{
    var originalService =sp.GetRequiredService<ProductServices>();

    var cache =sp.GetRequiredService<IDistributedCache>();

    var logger =sp.GetRequiredService<ILogger<CachedProductService>>();

    return new CachedProductService(originalService,cache,logger);
});

builder.Services.AddScoped<IOrderServices, OrderServices>();

// Strategy Pattern
builder.Services.AddScoped<NormalPricingStrategy>();
builder.Services.AddScoped<VipPricingStrategy>();
builder.Services.AddScoped<BlackFridayPricingStrategy>();

builder.Services.AddScoped<
    IPricingStrategyResolver,
    PricingStrategyResolver>();

// Factory Pattern
builder.Services.AddScoped<EmailNotification>();
builder.Services.AddScoped<SmsNotification>();
builder.Services.AddScoped<PushNotification>();
builder.Services.AddScoped<StockValidationHandler>();
builder.Services.AddScoped<
    INotificationFactory,
    NotificationFactory>();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddHealthChecks();

builder.Services.AddScoped<CustomerValidationHandler>();
builder.Services.AddScoped<ProductValidationHandler>();

builder.Services.AddScoped<IOrderValidationHandler>(sp =>
{
    var customer =
        sp.GetRequiredService<CustomerValidationHandler>();

    var product =
        sp.GetRequiredService<ProductValidationHandler>();
    var stock=
    sp.GetRequiredService<StockValidationHandler>();

    customer.SetNext(product);
    product.SetNext(stock);


    return customer;
});
builder.Services.AddScoped<FakePaymentPlatform>();
builder.Services.AddScoped<IPaymentService, PaymentAdapter>();

var app = builder.Build();

app.MapHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    await db.Database.MigrateAsync();
}

// HTTP Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/api/products/{id:int}",async (int id, IProductServices service) =>
    {
        var result = await service.GetProductByIdResultAsync(id);

        return result.Status switch
        {
            ResultStatus.Success =>Results.Ok(result.Value),

            ResultStatus.ValidationFailure => Results.BadRequest(result.Error),

            ResultStatus.NotFound =>Results.NotFound(result.Error),

            ResultStatus.Conflict =>Results.Conflict(result.Error),

            ResultStatus.Failure =>Results.Problem( title: "Operation failed",statusCode: 500),

            _ => Results.Problem()
        };
    });

app.Run();
