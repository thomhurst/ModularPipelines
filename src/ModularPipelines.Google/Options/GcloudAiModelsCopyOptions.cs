using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "models", "copy")]
public record GcloudAiModelsCopyOptions(
[property: CommandSwitch("--source-model")] string SourceModel
) : GcloudOptions
{
    [CommandSwitch("--kms-key-name")]
    public string? KmsKeyName { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--destination-model-id")]
    public string? DestinationModelId { get; set; }

    [CommandSwitch("--destination-parent-model")]
    public string? DestinationParentModel { get; set; }
}