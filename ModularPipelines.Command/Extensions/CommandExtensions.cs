using ModularPipelines.Context;

namespace ModularPipelines.Command.Extensions;

public static class CommandExtensions
{
    public static ICommand Command(this IModuleContext context) => context.Get<Command>()!;
}