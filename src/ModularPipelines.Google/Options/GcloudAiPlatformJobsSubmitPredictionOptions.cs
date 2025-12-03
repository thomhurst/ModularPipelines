using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "jobs", "submit", "prediction")]
public record GcloudAiPlatformJobsSubmitPredictionOptions(
[property: CliArgument] string Job,
[property: CliOption("--data-format")] string DataFormat,
[property: CliOption("--input-paths")] string[] InputPaths,
[property: CliOption("--output-path")] string OutputPath,
[property: CliOption("--region")] string Region,
[property: CliOption("--model")] string Model,
[property: CliOption("--model-dir")] string ModelDir
) : GcloudOptions
{
    [CliOption("--batch-size")]
    public string? BatchSize { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--max-worker-count")]
    public string? MaxWorkerCount { get; set; }

    [CliOption("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CliOption("--signature-name")]
    public string? SignatureName { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }
}