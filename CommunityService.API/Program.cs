using CommunityService.API.Exceptions;
using CommunityService.Application;
using CommunityService.Persistence;
using FastEndpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Logging.AddConsole();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFastEndpoints();

builder.Services.AddPersistence();
builder.Services.AddApplication();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(opts => opts.RouteTemplate = "openapi/{documentName}.json");
    app.MapScalarApiReference();
}

app.UseFastEndpoints();

app.UseHttpsRedirection();

app.Run();
