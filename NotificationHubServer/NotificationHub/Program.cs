using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NotificationHub.Domain.Data;
using NotificationHub.Domain.Hubs;
using NotificationHub.Domain.ProductService;
using NotificationHub.Domain.Seeding;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// SignalR:
builder.Services.AddSignalR();
builder.Services.AddScoped<IProductService, InMemoryProductService>();
builder.Services.AddScoped<IProductNotification, ProductNotification>();

// Baraa add and sqlServerConnStr is name of key in appSettings
    builder.Services.AddDbContext<AppDbContext>(option =>
    {
        option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
    });

// Enable Cors 'Baraa Add' For FrontEnd
    builder.Services.AddCors(c =>
    {
        c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
// SignalR:
app.MapHub<ProductNotificationHub>("/Notify");
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());




// To add Seeding data
using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
try
{
    AppDbContext context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
    StoreContextSeed.SeedAsync(context).Wait();
}
catch (Exception ex)
{
    serviceScope.ServiceProvider.GetRequiredService<ILogger<Program>>().LogError(ex, "Error Occured while migration process");
}

app.Run();
