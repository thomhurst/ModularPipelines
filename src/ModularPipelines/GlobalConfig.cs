using ModularPipelines.Enums;

namespace ModularPipelines;

public static class GlobalConfig
{
    public static CommandLogging DefaultCommandLogging { get; set; } =
        CommandLogging.Input | CommandLogging.Output | CommandLogging.Error;
}