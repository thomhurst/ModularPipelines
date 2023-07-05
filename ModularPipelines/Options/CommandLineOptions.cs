using CliWrap;

namespace ModularPipelines.Options;

public record CommandLineOptions
{
    public IDictionary<string, string?>? EnvironmentVariables { get; init; }
    public string? WorkingDirectory { get; init; }
    public Credentials? Credentials { get; init; }
    public bool LogInput { get; init; } = true;
    public bool LogOutput { get; init; } = true;
    
    public Func<string, string>? InputLoggingManipulator { get; init; }
    public Func<string, string>? OutputLoggingManipulator { get; init; }
}