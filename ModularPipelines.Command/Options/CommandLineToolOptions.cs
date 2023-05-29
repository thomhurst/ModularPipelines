namespace ModularPipelines.Command.Options;

public record CommandLineToolOptions(string Tool)
{
    public IEnumerable<string>? Arguments { get; init; }
    public IDictionary<string, string?>? EnvironmentVariables { get; init; }
    public string? WorkingDirectory { get; init; }
}