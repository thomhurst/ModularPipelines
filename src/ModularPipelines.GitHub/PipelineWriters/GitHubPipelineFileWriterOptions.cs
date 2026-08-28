using ModularPipelines.FileSystem;

namespace ModularPipelines.GitHub.PipelineWriters;

public record GitHubPipelineFileWriterOptions
{
    public required string Name { get; init; }

    public required TriggerCondition TriggerCondition { get; init; }

    public required FilePath OutputPath { get; init; }

    public required FilePath PipelineProjectPath { get; init; }

    public string? DotNetRunFramework { get; init; }

    /// <summary>
    /// Gets the .NET SDK version installed by the generated workflow.
    /// </summary>
    public string DotNetVersion { get; init; } = "10.0.x";

    public string Runner { get; init; } = "ubuntu-latest";

    public IEnumerable<string>? ValuesToMask { get; init; }

    public bool CacheNuGet { get; init; }

    public string? Environment { get; init; }

    public IDictionary<string, string>? EnvironmentVariables { get; init; }
}
