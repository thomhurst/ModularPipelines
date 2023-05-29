namespace ModularPipelines.Git.Options;

public record GitOptions
{
    public IDictionary<string, string?>? EnvironmentVariables { get; init; }
    public string? WorkingDirectory { get; init; }
}