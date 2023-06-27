using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

public class DotnetTestModule : Module
{
    protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await context.DotNet().Test(new DotNetOptions
        {
            WorkingDirectory = context.Environment.GitRootDirectory!.GetFolder("ModularPipelines.UnitTests").Path
        }, cancellationToken);

        return null;
    }
}