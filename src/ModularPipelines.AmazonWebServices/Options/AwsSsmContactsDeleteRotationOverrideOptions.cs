using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-contacts", "delete-rotation-override")]
public record AwsSsmContactsDeleteRotationOverrideOptions(
[property: CliOption("--rotation-id")] string RotationId,
[property: CliOption("--rotation-override-id")] string RotationOverrideId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}