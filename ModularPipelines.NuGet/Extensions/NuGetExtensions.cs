using ModularPipelines.Context;

namespace ModularPipelines.NuGet.Extensions;

public static class NuGetExtensions
{
    public static INuGet NuGet(this IModuleContext context) => context.Get<NuGet>()!;
}