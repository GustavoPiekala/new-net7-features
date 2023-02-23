using API.Extensions;
using API.Services;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DI
builder.Services.AddScoped<IDotNet7DependencyInjectionService, DotNet7DependencyInjectionService>();

//Cache
builder.Services.AddOutputCache();

//Rate limiting
builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: "testpolicy", options =>
    {
        options.PermitLimit = 4;
        options.Window = TimeSpan.FromSeconds(20);
        options.QueueLimit = 2;
        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    }));

var app = builder.Build();

app.UseOutputCache();

app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

ControllersMap.Map(app);

app.Run();
