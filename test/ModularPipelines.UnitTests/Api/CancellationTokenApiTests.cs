using ModularPipelines.Context;

namespace ModularPipelines.UnitTests.Api;

public class CancellationTokenApiTests
{
    [Test]
    public async Task PublicMethods_UseStandardCancellationTokenParameterName()
    {
        var nonstandardParameters = typeof(IModuleContext).Assembly
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
