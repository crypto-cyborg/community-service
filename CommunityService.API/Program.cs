using CommunityService.Persistence.Contexts;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFastEndpoints();

builder.Services.AddDbContext<CommunityContext>(opts =>
    opts.UseInMemoryDatabase("CommunityInMemo"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(opts => opts.RouteTemplate = "openapi/{documentName}.json");
    app.MapScalarApiReference();
}

app.UseFastEndpoints();

app.UseHttpsRedirection();

app.Run();