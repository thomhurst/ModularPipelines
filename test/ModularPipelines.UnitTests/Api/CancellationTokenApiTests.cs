using ModularPipelines.Context;
using ModularPipelines.Git;

namespace ModularPipelines.UnitTests.Api;

public class CancellationTokenApiTests
{
    [Test]
    public async Task PublicMethods_UseStandardCancellationTokenParameterName()
    {
        var assemblies = new[]
        {
            typeof(IModuleContext).Assembly,
            typeof(IGitCommands).Assembly,
        };

        var nonstandardParameters = assemblies
            .SelectMany(assembly => assembly.GetExportedTypes())
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
