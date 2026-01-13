using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.GitHub.PipelineWriters;

public record GitHubPipelineFileWriterOptions
{
    public required string Name { get; init; }

    public required TriggerCondition TriggerCondition { get; init; }

    public required File OutputPath { get; init; }

    public required File PipelineProjectPath { get; init; }

    public string? DotNetRunFramework { get; init; }

    public string Runner { get; init; } = "ubuntu-latest";

    public IEnumerable<string>? ValuesToMask { get; init; }

    public bool CacheNuGet { get; init; }

    public string? Environment { get; init; }

    public IDictionary<string, string>? EnvironmentVariables { get; init; }

    public OperatingSystemIdentifier RunnerOperatingSystem { get; init; } = OperatingSystemIdentifier.Linux;
}