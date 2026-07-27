using ModularPipelines.Node;

namespace ModularPipelines.Node.UnitTests.Api;

public class CancellationTokenApiTests
{
    [Test]
    public async Task PublicMethods_UseStandardCancellationTokenParameterName()
    {
        var nonstandardParameters = typeof(INpm).Assembly
            .GetExportedTypes()
            .SelectMany(type => type.GetMethods())
            .SelectMany(method => method.GetParameters())
            .Where(parameter => parameter.ParameterType == typeof(CancellationToken))
            .Where(parameter => parameter.Name != "cancellationToken")
            .Select(parameter => $"{parameter.Member.DeclaringType?.FullName}.{parameter.Member.Name}({parameter.Name})")
            .Distinct()
            .Order()
            .ToArray();

        await Assert.That(nonstandardParameters).IsEmpty();
    }
}
