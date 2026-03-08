using Microsoft.Extensions.Logging;
using Moq;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Matrix;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.UnitTests.Integration;

public class MatrixExpansionIntegrationTests
{
    [MatrixTarget("windows", "linux", "macos")]
    private class CrossPlatformModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            ModularPipelines.Context.IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("done");
        }
    }

    private class RegularModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            ModularPipelines.Context.IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("done");
        }
    }

    [Test]
    public async Task Matrix_Module_Expands_And_Creates_Capability_Assignments()
    {
        var logger = Mock.Of<ILogger<MatrixModuleExpander>>();
        var expander = new MatrixModuleExpander(logger);

        var module = new CrossPlatformModule();
        expander.ScanForExpansions(new IModule[] { module });

        var expansions = expander.GetExpansions(typeof(CrossPlatformModule));

        await Assert.That(expansions).IsNotNull();
        await Assert.That(expansions!.Count).IsEqualTo(3);
        await Assert.That(expansions[0].TargetValue).IsEqualTo("windows");
        await Assert.That(expansions[0].CapabilityName).IsEqualTo("windows");
        await Assert.That(expansions[1].TargetValue).IsEqualTo("linux");
        await Assert.That(expansions[1].CapabilityName).IsEqualTo("linux");
        await Assert.That(expansions[2].TargetValue).IsEqualTo("macos");
        await Assert.That(expansions[2].CapabilityName).IsEqualTo("macos");
    }

    [Test]
    public async Task Non_Matrix_Module_Is_Not_Expanded()
    {
        var logger = Mock.Of<ILogger<MatrixModuleExpander>>();
        var expander = new MatrixModuleExpander(logger);

        var module = new RegularModule();
        expander.ScanForExpansions(new IModule[] { module });

        var expansions = expander.GetExpansions(typeof(RegularModule));

        await Assert.That(expansions).IsNull();
    }

    [Test]
    public async Task Expanded_Module_Assignments_Have_Required_Capabilities()
    {
        var logger = Mock.Of<ILogger<MatrixModuleExpander>>();
        var expander = new MatrixModuleExpander(logger);

        var module = new CrossPlatformModule();
        expander.ScanForExpansions(new IModule[] { module });

        var expansions = expander.GetExpansions(typeof(CrossPlatformModule))!;

        // Each expansion should map to a capability-routed assignment
        foreach (var instance in expansions)
        {
            var assignment = new ModuleAssignment(
                ModuleTypeName: $"{instance.OriginalType.FullName}[{instance.TargetValue}]",
                ResultTypeName: typeof(string).FullName!,
                RequiredCapabilities: new HashSet<string>(StringComparer.OrdinalIgnoreCase) { instance.CapabilityName },
                MatrixTarget: instance.TargetValue,
                AssignedAt: DateTimeOffset.UtcNow,
                Configuration: new ModuleAssignmentConfig(null, 0, false));

            await Assert.That(assignment.RequiredCapabilities.Count).IsEqualTo(1);
            await Assert.That(assignment.RequiredCapabilities.Contains(instance.CapabilityName)).IsTrue();
            await Assert.That(assignment.MatrixTarget).IsEqualTo(instance.TargetValue);
        }
    }
}
