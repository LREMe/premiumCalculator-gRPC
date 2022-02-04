using gRPCPremiumCalculator.Data;
using gRPCPremiumCalculator.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "DataSource=Data\\app.db";

/// <summary>
///  SQL Lite Support
/// </summary>
builder.Services.AddSqlite<PremiumRolDbContext>(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<PeriodCRUDService>();
app.MapGrpcService<StateCRUDService>();
app.MapGrpcService<PlanCRUDService>();
app.MapGrpcService<PremiumRolCRUDService>();


app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
