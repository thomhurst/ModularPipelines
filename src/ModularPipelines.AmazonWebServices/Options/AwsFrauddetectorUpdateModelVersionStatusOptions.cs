using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "update-model-version-status")]
public record AwsFrauddetectorUpdateModelVersionStatusOptions(
[property: CommandSwitch("--model-id")] string ModelId,
[property: CommandSwitch("--model-type")] string ModelType,
[property: CommandSwitch("--model-version-number")] string ModelVersionNumber,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}