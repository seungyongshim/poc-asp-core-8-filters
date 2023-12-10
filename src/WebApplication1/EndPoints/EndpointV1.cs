using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApplication1.EndPoints;

public static class EndpointV1
{
    public static Results<Ok<Dto>, NotFound> NewMethod(Dto entity, CancellationToken cancellationToken) => TypedResults.Ok(entity);
}
