using System.Reflection;
using Application.Helpers;
using GarbageCollector.Managers;
using Infrastructure.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
  {
    Title = "Order Actor Garbage Collector API",
    Version = "v1",
    Description = "A demo API for cleaning up actors within Dapr."
  });

  var apiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  var apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXmlFile);
  c.IncludeXmlComments(apiXmlPath);
  c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Application.xml"));
  c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Domain.xml"));
  c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Infrastructure.xml"));
});
builder.Services.AddDaprClient();
builder.Services.AddHttpClient();

// Dependency injection
builder.Services.AddTransient<IGarbageCollectorManager, GarbageCollectorManager>();
builder.Services.AddTransient<IAesEncryptionHelper, AesEncryptionHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();
app.MapSubscribeHandler();
app.Run();