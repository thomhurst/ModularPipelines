using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "predict")]
public record GcloudMlEnginePredictOptions(
[property: CliOption("--model")] string Model,
[property: CliOption("--json-instances")] string JsonInstances,
[property: CliOption("--json-request")] string JsonRequest,
[property: CliOption("--text-instances")] string TextInstances
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--signature-name")]
    public string? SignatureName { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }
}