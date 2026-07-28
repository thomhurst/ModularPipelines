using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Models;

public class ModuleDependencyModelTests
{
    [Test]
    public async Task AllDescendantDependencies_ReturnsEachReachableModuleOnce()
    {
        var root = CreateModel<RootModule>();
        var left = CreateModel<LeftModule>();
        var right = CreateModel<RightModule>();
        var sharedLeaf = CreateModel<SharedLeafModule>();

        root.IsDependentOn.AddRange([left, right]);
        left.IsDependentOn.Add(sharedLeaf);
        right.IsDependentOn.Add(sharedLeaf);

        var descendants = root.AllDescendantDependencies().ToList();

        await Assert.That(descendants).IsEquivalentTo([left, right, sharedLeaf]);
    }

    private static ModuleDependencyModel CreateModel<T>()
        where T : IModule, new()
    {
        return new ModuleDependencyModel(new T());
    }

    private abstract class TestModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    private sealed class RootModule : TestModule;

    private sealed class LeftModule : TestModule;

    private sealed class RightModule : TestModule;

    private sealed class SharedLeafModule : TestModule;
}
