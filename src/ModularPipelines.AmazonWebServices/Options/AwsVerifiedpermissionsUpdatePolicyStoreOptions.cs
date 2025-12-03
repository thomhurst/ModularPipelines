using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("verifiedpermissions", "update-policy-store")]
public record AwsVerifiedpermissionsUpdatePolicyStoreOptions(
[property: CliOption("--policy-store-id")] string PolicyStoreId,
[property: CliOption("--validation-settings")] string ValidationSettings
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}