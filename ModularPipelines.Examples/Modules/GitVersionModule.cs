using ModularPipelines.Context;
using ModularPipelines.Git;

namespace ModularPipelines.Examples.Modules;

public class GitVersionModule : GitModule
{
    public GitVersionModule(IModuleContext context) : base(context)
    {
    }

    protected override IEnumerable<string> Arguments
    {
        get { yield return "--version"; }
    }
}