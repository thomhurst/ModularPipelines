using ModularPipelines.Attributes;
using ModularPipelines.Build.Modules.UnitTests;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[DependsOnAllModulesInheritingFrom<RunUnitTestModule>]
public class RunAllUnitTestsModule : Module<bool>
{
    protected override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}
