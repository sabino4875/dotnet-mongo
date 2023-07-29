using Api.CoronaVirusStatistics.EndPoints;
using Api.CoronaVirusStatistics.IoC;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.AddInfraStructure();

var app = builder.Build();
app.MapInfraStructure();

app.MapInfectadoEndPoints();

app.Run("https://localhost:3000");