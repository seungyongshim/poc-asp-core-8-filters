using System.Xml;
using FluentValidation;
using FluentValidation.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using WebApplication1.Handlers;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddFluentValidationEndpointFilter();
builder.Services.AddScoped<IValidator<Dto>, DtoValidator>();
//https://www.milanjovanovic.tech/blog/global-error-handling-in-aspnetcore-8
builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// https://learn.microsoft.com/ko-kr/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0#produce-a-problemdetails-payload-for-exceptions
builder.Services.AddProblemDetails();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler();
app.UseHttpsRedirection();



app.MapPost("/", (Dto entity, CancellationToken cancellationToken) => TypedResults.Ok(entity))
   .AddEndpointFilter<FluentValidationEndpointFilter>();

app.Run();

public record Dto
{
    public required string Name { get; init; }
}
public class DtoValidator : AbstractValidator<Dto>
{
    public DtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
