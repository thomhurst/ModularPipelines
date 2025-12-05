using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "local", "train")]
public record GcloudAiPlatformLocalTrainOptions(
[property: CliArgument] string UserArgs,
[property: CliOption("--module-name")] string ModuleName
) : GcloudOptions
{
    [CliFlag("--distributed")]
    public bool? Distributed { get; set; }

    [CliOption("--evaluator-count")]
    public string? EvaluatorCount { get; set; }

    [CliOption("--job-dir")]
    public string? JobDir { get; set; }

    [CliOption("--package-path")]
    public string? PackagePath { get; set; }

    [CliOption("--parameter-server-count")]
    public string? ParameterServerCount { get; set; }

    [CliOption("--start-port")]
    public string? StartPort { get; set; }

    [CliOption("--worker-count")]
    public string? WorkerCount { get; set; }
}