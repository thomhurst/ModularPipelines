using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml-engine", "jobs", "submit", "prediction")]
public record GcloudMlEngineJobsSubmitPredictionOptions(
[property: PositionalArgument] string Job,
[property: CommandSwitch("--data-format")] string DataFormat,
[property: CommandSwitch("--input-paths")] string[] InputPaths,
[property: CommandSwitch("--output-path")] string OutputPath,
[property: CommandSwitch("--region")] string Region,
[property: CommandSwitch("--model")] string Model,
[property: CommandSwitch("--model-dir")] string ModelDir
) : GcloudOptions
{
    [CommandSwitch("--batch-size")]
    public string? BatchSize { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--max-worker-count")]
    public string? MaxWorkerCount { get; set; }

    [CommandSwitch("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CommandSwitch("--signature-name")]
    public string? SignatureName { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }
}