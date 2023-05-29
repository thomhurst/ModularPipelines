using ModularPipelines.Context;

namespace ModularPipelines.DotNet.Extensions;

public static class DotNetExtensions
{
    public static IDotNet DotNet(this IModuleContext context) => context.Get<DotNet>()!;
}