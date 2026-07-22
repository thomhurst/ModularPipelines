using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Dependencies;

public class ModuleDependencyResolverTests
{
    private sealed class DependencyModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("dependency");
    }

    [ModularPipelines.Attributes.DependsOn<DependencyModule>]
    private sealed class DirectModule : IModule
    {
        public Type ResultType => typeof(string);

        public ModuleConfiguration Configuration { get; } = ModuleConfiguration.Create()
            .DependsOnOptional<DependencyModule>()
            .Build();

        public Task<IModuleResult> ResultTask => null!;

        public bool TrySetDistributedResult(IModuleResult result) => false;
    }

    [Test]
    public async Task GetAllDependencies_IncludesAttributesForDirectModules()
    {
        var dependencies = ModuleDependencyResolver
            .GetAllDependencies(
                new DirectModule(),
                [typeof(DirectModule), typeof(DependencyModule)])
            .ToArray();

        await Assert.That(dependencies).Contains((typeof(DependencyModule), false));
        await Assert.That(dependencies).Count().IsEqualTo(1);
    }
}
