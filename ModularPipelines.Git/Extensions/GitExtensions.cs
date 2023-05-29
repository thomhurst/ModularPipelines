using ModularPipelines.Context;

namespace ModularPipelines.Git.Extensions;

public static class GitExtensions
{
    public static IGit Git(this IModuleContext context) => context.Get<Git>()!;
}