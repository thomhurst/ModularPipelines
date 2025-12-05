using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("verifiedpermissions", "delete-policy")]
public record AwsVerifiedpermissionsDeletePolicyOptions(
[property: CliOption("--policy-store-id")] string PolicyStoreId,
[property: CliOption("--policy-id")] string PolicyId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}