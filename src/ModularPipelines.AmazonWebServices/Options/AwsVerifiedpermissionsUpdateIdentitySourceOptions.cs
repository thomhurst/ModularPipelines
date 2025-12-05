using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("verifiedpermissions", "update-identity-source")]
public record AwsVerifiedpermissionsUpdateIdentitySourceOptions(
[property: CliOption("--policy-store-id")] string PolicyStoreId,
[property: CliOption("--identity-source-id")] string IdentitySourceId,
[property: CliOption("--update-configuration")] string UpdateConfiguration
) : AwsOptions
{
    [CliOption("--principal-entity-type")]
    public string? PrincipalEntityType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}