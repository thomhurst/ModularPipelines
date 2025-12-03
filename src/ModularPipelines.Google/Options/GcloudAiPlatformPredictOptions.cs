using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "predict")]
public record GcloudAiPlatformPredictOptions(
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