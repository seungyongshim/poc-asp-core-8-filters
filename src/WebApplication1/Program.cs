using System.Xml;
using FluentValidation;
using FluentValidation.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddFluentValidationEndpointFilter();
builder.Services.AddScoped<IValidator<Dto>, DtoValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/", (Dto entity, CancellationToken cancellationToken) => TypedResults.Ok(entity))
   .AddEndpointFilter<FluentValidationEndpointFilter>()
;

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
