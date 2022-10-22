using Application.Managers;
using Application.Repositories;
using Infrastructure.Actors;
using Infrastructure.Managers;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddActors(options =>
{
  // Register actor types and configure actor settings
  options.Actors.RegisterActor<OrderActor>();
});
builder.Services.AddDaprClient();

// Dependency injection
builder.Services.AddTransient<IOrderManager, OrderManager>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();
app.MapActorsHandlers();
app.Run();

