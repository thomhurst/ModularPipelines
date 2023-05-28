using ModularPipelines.Command;
using ModularPipelines.Context;

namespace ModularPipelines.Git;

public abstract class GitModule : CommandLineToolModule
{
    protected GitModule(IModuleContext context) : base(context)
    {
    }

    protected override string Tool => "git";
}