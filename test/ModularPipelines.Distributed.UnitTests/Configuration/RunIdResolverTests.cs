using ModularPipelines.Distributed.Configuration;

namespace ModularPipelines.Distributed.UnitTests.Configuration;

[TUnit.Core.NotInParallel("ProcessEnvironment")]
public class RunIdResolverTests
{
    [Test]
    public async Task Resolve_Returns_Explicit_Value()
    {
        var result = RunIdResolver.Resolve("explicit-run");

        await Assert.That(result).IsEqualTo("explicit-run");
    }

    [Test]
    public async Task Resolve_Returns_Standard_Environment_Value()
    {
        var original = Environment.GetEnvironmentVariable(RunIdResolver.EnvironmentVariable);
        try
        {
            Environment.SetEnvironmentVariable(RunIdResolver.EnvironmentVariable, "environment-run");

            var result = RunIdResolver.Resolve(null);

            await Assert.That(result).IsEqualTo("environment-run");
        }
        finally
        {
            Environment.SetEnvironmentVariable(RunIdResolver.EnvironmentVariable, original);
        }
    }

    [Test]
    public async Task Resolve_Generates_Value_When_Unconfigured()
    {
        var original = Environment.GetEnvironmentVariable(RunIdResolver.EnvironmentVariable);
        try
        {
            Environment.SetEnvironmentVariable(RunIdResolver.EnvironmentVariable, null);

            var result = RunIdResolver.Resolve(" ");

            await Assert.That(Guid.TryParseExact(result, "N", out _)).IsTrue();
        }
        finally
        {
            Environment.SetEnvironmentVariable(RunIdResolver.EnvironmentVariable, original);
        }
    }
}
