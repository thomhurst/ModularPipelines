using Microsoft.Extensions.Logging;
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
        var gitVersioning = await context.Git().Versioning.GetGitVersioningInformation();

        var gitVersion = gitVersioning.FullSemVer;
        
        context.Logger.LogInformation("Git Version {Version}", gitVersion);

        return await context.Git().Commands.Git(new GitBaseOptions
        {
            Version = true
        }, token: cancellationToken);
    }
}
