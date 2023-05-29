namespace ModularPipelines.Command.Options;

public record CommandOptions(string Command)
{
    public IReadOnlyDictionary<string, string?>? EnvironmentVariables { get; init; }
    public string? WorkingDirectory { get; init; }
}