using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.GitHub.PipelineWriters;

public record GitHubPipelineFileWriterOptions
{
    public string Name { get; init; } = null!;

    public TriggerCondition TriggerCondition { get; init; } = null!;

    public File OutputPath { get; init; } = null!;

    public File PipelineProjectPath { get; init; } = null!;

    public string? DotNetRunFramework { get; init; }

    public string Runner { get; init; } = "ubuntu-latest";

    public IEnumerable<string>? ValuesToMask { get; init; }

    public bool CacheNuGet { get; init; }

    public string? Environment { get; init; }

    public IDictionary<string, string>? EnvironmentVariables { get; init; }

    public OperatingSystemIdentifier RunnerOperatingSystem { get; init; } = OperatingSystemIdentifier.Linux;
}