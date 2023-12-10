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
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// https://learn.microsoft.com/ko-kr/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0#produce-a-problemdetails-payload-for-exceptions
builder.Services.AddProblemDetails();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler(exceptionHandlerApp =>
{
    https://datatracker.ietf.org/doc/html/rfc7807
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = Text.Plain;

        var title = "Bad Input";
        var detail = "Invalid input";
        var type = "https://errors.example.com/badInput";

        if (context.RequestServices.GetService<IProblemDetailsService>() is
            { } problemDetailsService)
        {
            var exceptionHandlerFeature =
           context.Features.Get<IExceptionHandlerFeature>();

            var exceptionType = exceptionHandlerFeature?.Error;
            if (exceptionType != null &&
               exceptionType.Message.Contains("infinity"))
            {
                title = "Argument exception";
                detail = "Invalid input";
                type = "https://errors.example.com/argumentException";
            }

            await problemDetailsService.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails =
                {
                    Title = title,
                    Detail = detail,
                    Type = type
                }
            });
        }
    });
});
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
