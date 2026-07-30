using ModularPipelines.Attributes;
using ModularPipelines.Build.Modules.UnitTests;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

#pragma warning disable MPG0016 // The build pipeline is JIT-only.
[DependsOnAllModulesInheritingFrom<RunUnitTestModule>]
#pragma warning restore MPG0016
public class RunAllUnitTestsModule : Module<bool>
{
    protected override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}
