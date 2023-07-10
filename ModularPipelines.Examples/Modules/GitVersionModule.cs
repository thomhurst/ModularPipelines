using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

public class GitVersionModule : Module<CommandResult>
{
    protected override async Task<ModuleResult<CommandResult>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return await context.Git().Commands.Git(new GitBaseOptions
        {
            Version = true
        }, token: cancellationToken);
    }
}
