using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("verifiedpermissions", "get-policy")]
public record AwsVerifiedpermissionsGetPolicyOptions(
[property: CliOption("--policy-store-id")] string PolicyStoreId,
[property: CliOption("--policy-id")] string PolicyId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}