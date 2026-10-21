using ModularPipelines.FileSystem;

namespace ModularPipelines.GitHub;

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
    public string DotNetVersion { get; init; } = "11.0.x";

    public string Runner { get; init; } = "ubuntu-latest";

    /// <summary>
    /// Gets the GitHub secret names whose values the generated workflow masks, for example <c>NUGET_API_KEY</c>.
    /// </summary>
    /// <remarks>
    /// Supply names, not secret values or expressions. Names must start with an ASCII letter or underscore
    /// and contain only ASCII letters, digits, or underscores. The writer emits <c>${{ secrets.NAME }}</c>
    /// references; GitHub Actions resolves the values at runtime.
    /// </remarks>
    public IEnumerable<string>? SecretNamesToMask { get; init; }

    public bool CacheNuGet { get; init; }

    public string? Environment { get; init; }

    public IDictionary<string, string>? EnvironmentVariables { get; init; }
}
