using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-contacts", "delete-rotation-override")]
public record AwsSsmContactsDeleteRotationOverrideOptions(
[property: CommandSwitch("--rotation-id")] string RotationId,
[property: CommandSwitch("--rotation-override-id")] string RotationOverrideId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}