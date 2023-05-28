using Pipeline.NET.Context;
using Pipeline.NET.DotNet.Options;

namespace Pipeline.NET.DotNet.Modules;

public class ExternalRunnableDotNetCommandModule : DotNetCommandModule
{
    public ExternalRunnableDotNetCommandModule(IModuleContext context, 
        DotNetCommandModuleOptions options) : base(context)
    {
        Options = options;
    }

    protected override DotNetCommandModuleOptions Options { get; }
}