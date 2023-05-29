namespace ModularPipelines.Git.Options;

public record GitArgumentOptions(IEnumerable<string> Arguments)
{
    public IDictionary<string, string?>? EnvironmentVariables { get; init; }
    public string? WorkingDirectory { get; init; }
}