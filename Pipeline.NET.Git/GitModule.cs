using Pipeline.NET.Command;
using Pipeline.NET.Context;

namespace Pipeline.NET.Git;

public abstract class GitModule : CommandLineToolModule
{
    protected GitModule(IModuleContext context) : base(context)
    {
    }

    protected override string Tool => "git";
}