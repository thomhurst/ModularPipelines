using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;

namespace ModularPipelines.DotNet.Modules;

public class ExternalRunnableDotNetCommandModule : DotNetCommandModule
{
    public ExternalRunnableDotNetCommandModule(IModuleContext context, 
        DotNetCommandModuleOptions options) : base(context)
    {
        Options = options;
    }

    protected override DotNetCommandModuleOptions Options { get; set; }
}