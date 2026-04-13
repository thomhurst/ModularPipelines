using Microsoft.Extensions.Logging;
using Moq;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Matrix;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.UnitTests.Matrix;

public class MatrixModuleExpanderTests
{
    [MatrixTarget("windows", "linux", "macos")]
    private class CrossPlatformModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            ModularPipelines.Context.IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("result");
        }
    }

    private class RegularModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            ModularPipelines.Context.IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("result");
        }
    }

    [Test]
    public async Task ScanForExpansions_Expands_MatrixTarget_Module()
    {
        var logger = Mock.Of<ILogger<MatrixModuleExpander>>();
        var expander = new MatrixModuleExpander(logger);

        var module = new CrossPlatformModule();
        expander.ScanForExpansions(new[] { module });

        var expansions = expander.GetExpansions(typeof(CrossPlatformModule));

        await Assert.That(expansions).IsNotNull();
        await Assert.That(expansions!.Count).IsEqualTo(3);
    }

    [Test]
    public async Task ScanForExpansions_No_Expansion_For_Regular_Module()
    {
        var logger = Mock.Of<ILogger<MatrixModuleExpander>>();
        var expander = new MatrixModuleExpander(logger);

        var module = new RegularModule();
        expander.ScanForExpansions(new[] { module });

        var expansions = expander.GetExpansions(typeof(RegularModule));

        await Assert.That(expansions).IsNull();
    }

    [Test]
    public async Task Expanded_Instances_Have_Correct_Metadata()
    {
        var logger = Mock.Of<ILogger<MatrixModuleExpander>>();
        var expander = new MatrixModuleExpander(logger);

        var module = new CrossPlatformModule();
        expander.ScanForExpansions(new[] { module });

        var expansions = expander.GetExpansions(typeof(CrossPlatformModule))!;

        await Assert.That(expansions[0].TargetValue).IsEqualTo("windows");
        await Assert.That(expansions[0].CapabilityName).IsEqualTo("windows");
        await Assert.That(expansions[0].InstanceName).IsEqualTo("CrossPlatformModule[windows]");
        await Assert.That(expansions[1].TargetValue).IsEqualTo("linux");
        await Assert.That(expansions[2].TargetValue).IsEqualTo("macos");
    }
}
