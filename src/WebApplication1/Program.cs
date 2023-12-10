using FluentValidation;
using FluentValidation.AspNetCore.Http;
using WebApplication1.EndPoints;
using WebApplication1.Handlers;

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
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}
app.UseExceptionHandler();
app.UseHttpsRedirection();

app.MapPost("/", EndpointV1.NewMethod)
   .AddEndpointFilter<FluentValidationEndpointFilter>();

app.Run();

public record Dto
{
    public string? Id { get; init; }
    public required string Name { get; init; }
}
public class DtoValidator : AbstractValidator<Dto>
{
    public DtoValidator() => RuleFor(x => x.Name).NotEmpty();
}


