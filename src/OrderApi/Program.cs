using System.Reflection;
using Application.Helpers;
using Application.Managers;
using Application.Repositories;
using Infrastructure.Actors;
using Infrastructure.Helpers;
using Infrastructure.Managers;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
  {
    Title = "Order API",
    Version = "v1",
    Description = "A demo API for interacting with various Dapr building blocks while processing orders."
  });

  var apiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  var apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXmlFile);
  c.IncludeXmlComments(apiXmlPath);
  c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Application.xml"));
  c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Domain.xml"));
  c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Infrastructure.xml"));
});
builder.Services.AddActors(options =>
{
  // Register actor types and configure actor settings
  options.Actors.RegisterActor<OrderActor>();
});
builder.Services.AddDaprClient();

// Dependency injection
builder.Services.AddTransient<IOrderManager, OrderManager>();
builder.Services.AddTransient<IAesEncryptionHelper, AesEncryptionHelper>();
builder.Services.AddTransient<IOrderPubSubRepository, OrderPubSubRepository>();
builder.Services.AddTransient<IOrderStateRepository, OrderStateRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapActorsHandlers();
app.UseRouting();
app.MapControllers();
app.Run();

