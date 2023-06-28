using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

public class GitVersionModule : Module<BufferedCommandResult>
{
    protected override async Task<ModuleResult<BufferedCommandResult>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return await context.Git().Operations.Version(cancellationToken: cancellationToken);
    }
}