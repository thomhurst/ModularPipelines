namespace ModularPipelines.Options;

/// <summary>
/// Generic command-line tool options for running arbitrary command-line tools.
/// </summary>
public record GenericCommandLineToolOptions : CommandLineToolOptions
{
    /// <summary>
    /// Creates options for the specified command-line tool.
    /// </summary>
    /// <param name="tool">The name or path of the command-line tool to execute.</param>
    public GenericCommandLineToolOptions(string tool)
    {
        Tool = tool;
    }
}
