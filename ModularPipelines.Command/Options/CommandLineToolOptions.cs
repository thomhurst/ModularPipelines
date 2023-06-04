namespace ModularPipelines.Command.Options;

public record CommandLineToolOptions(string Tool) : CommandEnvironmentOptions
{
    public IEnumerable<string>? Arguments { get; init; }
}