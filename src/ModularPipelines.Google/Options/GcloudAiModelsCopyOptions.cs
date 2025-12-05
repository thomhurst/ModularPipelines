using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "models", "copy")]
public record GcloudAiModelsCopyOptions(
[property: CliOption("--source-model")] string SourceModel
) : GcloudOptions
{
    [CliOption("--kms-key-name")]
    public string? KmsKeyName { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--destination-model-id")]
    public string? DestinationModelId { get; set; }

    [CliOption("--destination-parent-model")]
    public string? DestinationParentModel { get; set; }
}