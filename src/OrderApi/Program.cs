﻿using System.Reflection;
using Application.GarbageCollector;
using Application.Helpers;
using Application.Repositories;
using Infrastructure.GarbageCollector;
using Infrastructure.Helpers;
using Infrastructure.Repositories;
using OrderApi.Actors;
using OrderApi.Managers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ActorConfig>(builder.Configuration.GetSection(nameof(ActorConfig)));

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

  // Setup actor config
  var actorConfig = builder.Configuration.GetSection(nameof(ActorConfig)).Get<ActorConfig>();
  options.ActorIdleTimeout = TimeSpan.FromMinutes(actorConfig!.ActorIdleTimeout);
  options.ActorScanInterval = TimeSpan.FromSeconds(actorConfig!.ActorScanInterval);
  options.DrainOngoingCallTimeout = TimeSpan.FromSeconds(actorConfig!.DrainOngoingCallTimeout);
  options.DrainRebalancedActors = actorConfig!.DrainRebalancedActors;
  options.RemindersStoragePartitions = actorConfig!.RemindersStoragePartitions;
  options.ReentrancyConfig = actorConfig!.ReentrancyConfig;
});
builder.Services.AddDaprClient();
builder.Services.AddHttpClient();

// Dependency injection
builder.Services.AddTransient<IOrderManager, OrderManager>();
builder.Services.AddTransient<IAesEncryptionHelper, AesEncryptionHelper>();
builder.Services.AddTransient<IActorGarbageCollector, ActorGarbageCollector>();
builder.Services.AddTransient<IOrderPubSubRepository, OrderPubSubRepository>();
builder.Services.AddTransient<IOrderStateRepository, OrderStateRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.MapActorsHandlers();
app.UseRouting();
app.MapControllers();
app.Run();