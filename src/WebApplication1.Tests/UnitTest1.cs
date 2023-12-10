using WebApplication1.EndPoints;

namespace WebApplication1.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        using var cts = new CancellationTokenSource();

        var res = EndpointV1.NewMethod(new Dto
        {
            Id = "1",
            Name = ""
        }, cts.Token);

        res.
    }
}
