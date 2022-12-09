using Dapr.Client;
using Dapr.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

using var client = new DaprClientBuilder().Build();
builder.Configuration.AddDaprSecretStore("secrets", client, TimeSpan.FromSeconds(5));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
