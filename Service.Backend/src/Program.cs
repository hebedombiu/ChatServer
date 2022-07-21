using Microsoft.Extensions.DependencyInjection.Extensions;
using Service.Backend.Chat;
using Service.Backend.Connections;
using Service.Backend.Handlers;
using Service.Backend.Options;
using Service.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoOptions>(builder.Configuration.GetSection("Mongo"));
builder.Services.Configure<ServerOptions>(builder.Configuration.GetSection("Server"));
builder.Services.Configure<StaticOptions>(builder.Configuration.GetSection("Static"));

builder.Services.TryAddSingleton<HandlersService>();
builder.Services.TryAddSingleton<StaticService>();
builder.Services.TryAddSingleton<ServerService>();
builder.Services.TryAddSingleton<MongoService>();

builder.Services.TryAddSingleton<ConnectionHandler>();
builder.Services.TryAddSingleton<SendMessageHandler>();

builder.Services.TryAddSingleton<ChatRepository>();
builder.Services.TryAddSingleton<ChatService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// warm up
app.Services.GetService<ServerService>();

if (app.Environment.IsDevelopment()) { }

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();