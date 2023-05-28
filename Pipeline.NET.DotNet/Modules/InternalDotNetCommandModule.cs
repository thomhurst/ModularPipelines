using Pipeline.NET.Context;
using Pipeline.NET.DotNet.Options;

namespace Pipeline.NET.DotNet.Modules;

internal class InternalDotNetCommandModule : DotNetCommandModule
{
    public InternalDotNetCommandModule(IModuleContext context, 
        DotNetCommandModuleOptions options) : base(context)
    {
        Options = options;
    }

    protected override DotNetCommandModuleOptions Options { get; }
}