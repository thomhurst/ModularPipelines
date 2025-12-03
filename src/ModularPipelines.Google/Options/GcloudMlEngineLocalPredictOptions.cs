using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "local", "predict")]
public record GcloudMlEngineLocalPredictOptions(
[property: CliOption("--model-dir")] string ModelDir,
[property: CliOption("--json-instances")] string JsonInstances,
[property: CliOption("--json-request")] string JsonRequest,
[property: CliOption("--text-instances")] string TextInstances
) : GcloudOptions
{
    [CliOption("--framework")]
    public string? Framework { get; set; }

    [CliOption("--signature-name")]
    public string? SignatureName { get; set; }
}