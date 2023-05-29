using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

public class GitVersionModule : Module<BufferedCommandResult>
{
    public GitVersionModule(IModuleContext context) : base(context)
    {
    }

    protected override async Task<ModuleResult<BufferedCommandResult>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        return await Context.Git().Version();
    }
}