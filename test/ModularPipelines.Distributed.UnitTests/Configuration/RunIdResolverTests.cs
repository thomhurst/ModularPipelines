using ModularPipelines.Distributed.Configuration;

namespace ModularPipelines.Distributed.UnitTests.Configuration;

[TUnit.Core.NotInParallel("ProcessEnvironment")]
public class RunIdResolverTests
{
    [Test]
    public async Task Resolve_Returns_Explicit_Value()
    {
        var result = RunIdResolver.Resolve("explicit-run", totalInstances: 2);

        await Assert.That(result).IsEqualTo("explicit-run");
    }

    [Test]
    public async Task Resolve_Returns_Standard_Environment_Value()
    {
        var original = Environment.GetEnvironmentVariable(RunIdResolver.EnvironmentVariable);
        try
        {
            Environment.SetEnvironmentVariable(RunIdResolver.EnvironmentVariable, "environment-run");

            var result = RunIdResolver.Resolve(null, totalInstances: 2);

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

            var result = RunIdResolver.Resolve(" ", totalInstances: 1);

            await Assert.That(Guid.TryParseExact(result, "N", out _)).IsTrue();
        }
        finally
        {
            Environment.SetEnvironmentVariable(RunIdResolver.EnvironmentVariable, original);
        }
    }

    [Test]
    public async Task Resolve_Rejects_Unconfigured_Multi_Instance_Run()
    {
        var original = Environment.GetEnvironmentVariable(RunIdResolver.EnvironmentVariable);
        try
        {
            Environment.SetEnvironmentVariable(RunIdResolver.EnvironmentVariable, null);

            var exception = Assert.Throws<InvalidOperationException>(() =>
                RunIdResolver.Resolve(null, totalInstances: 2));

            using (Assert.Multiple())
            {
                await Assert.That(exception.Message).Contains(nameof(DistributedOptions.RunId));
                await Assert.That(exception.Message).Contains(RunIdResolver.EnvironmentVariable);
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable(RunIdResolver.EnvironmentVariable, original);
        }
    }

    [Test]
    public async Task Resolve_Rejects_Unconfigured_Run_When_Explicit_Id_Is_Required()
    {
        var original = Environment.GetEnvironmentVariable(RunIdResolver.EnvironmentVariable);
        try
        {
            Environment.SetEnvironmentVariable(RunIdResolver.EnvironmentVariable, null);

            var exception = Assert.Throws<InvalidOperationException>(() =>
                RunIdResolver.Resolve(null, totalInstances: 1, requireExplicitRunId: true));

            using (Assert.Multiple())
            {
                await Assert.That(exception.Message).Contains(nameof(DistributedOptions.RunId));
                await Assert.That(exception.Message).Contains(RunIdResolver.EnvironmentVariable);
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable(RunIdResolver.EnvironmentVariable, original);
        }
    }
}
