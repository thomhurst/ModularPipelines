namespace ModularPipelines.Engine;

/// <summary>
/// Centralizes routing for build-system group commands.
/// </summary>
internal static class BuildSystemFormatterExtensions
{
    /// <summary>
    /// Writes a group command without rendering raw CI commands through Spectre.
    /// </summary>
    /// <param name="formatter">The active build-system formatter.</param>
    /// <param name="command">The group command, or <see langword="null"/> when unsupported.</param>
    /// <param name="rawWriter">Writes commands that must bypass rendering and interception.</param>
    /// <param name="fallbackWriter">Writes locally rendered group headers.</param>
    public static void WriteGroupCommand(
        this IBuildSystemFormatter formatter,
        string? command,
        Action<string> rawWriter,
        Action<string> fallbackWriter)
    {
        if (command is null)
        {
            return;
        }

        if (formatter.UsesRawCommands)
        {
            rawWriter(command);
        }
        else
        {
            fallbackWriter(command);
        }
    }
}
