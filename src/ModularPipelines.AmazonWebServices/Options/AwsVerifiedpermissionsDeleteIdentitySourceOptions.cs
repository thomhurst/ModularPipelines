using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("verifiedpermissions", "delete-identity-source")]
public record AwsVerifiedpermissionsDeleteIdentitySourceOptions(
[property: CliOption("--policy-store-id")] string PolicyStoreId,
[property: CliOption("--identity-source-id")] string IdentitySourceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}