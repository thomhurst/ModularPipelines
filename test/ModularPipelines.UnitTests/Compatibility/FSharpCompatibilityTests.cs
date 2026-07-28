using ModularPipelines.Enums;
using ModularPipelines.FSharp.TestFixtures;

namespace ModularPipelines.UnitTests.Compatibility;

public class FSharpCompatibilityTests
{
    [Test]
    public async Task FSharp_Modules_Execute_With_Type_Based_Dependency()
    {
        var summary = await PipelineRunner.RunAsync();

        var dependencyResult = await summary.GetModule<DependencyModule>();
        var dependentResult = await summary.GetModule<DependentModule>();

        using (Assert.Multiple())
        {
            await Assert.That(summary.Status).IsEqualTo(Status.Successful);
            await Assert.That(dependencyResult.ValueOrDefault).IsEqualTo("dependency");
            await Assert.That(dependentResult.ValueOrDefault).IsEqualTo("dependency-dependent");
        }
    }

    [Test]
    public async Task FSharp_Modules_Execute_With_Dynamic_Dependency()
    {
        var summary = await PipelineRunner.RunDynamicAsync();

        var dependencyResult = await summary.GetModule<DependencyModule>();
        var dependentResult = await summary.GetModule<DynamicDependentModule>();

        using (Assert.Multiple())
        {
            await Assert.That(summary.Status).IsEqualTo(Status.Successful);
            await Assert.That(dependencyResult.ValueOrDefault).IsEqualTo("dependency");
            await Assert.That(dependentResult.ValueOrDefault).IsEqualTo("dependency-dynamic");
        }
    }
}
