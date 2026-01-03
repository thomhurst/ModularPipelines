namespace ModularPipelines.Options;

/// <summary>
/// Generic command-line tool options for running arbitrary command-line tools.
/// </summary>
/// <param name="Tool">The name or path of the command-line tool to execute.</param>
public record GenericCommandLineToolOptions(string Tool) : CommandLineToolOptions(Tool);
