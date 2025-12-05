using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("verifiedpermissions", "update-policy")]
public record AwsVerifiedpermissionsUpdatePolicyOptions(
[property: CliOption("--policy-store-id")] string PolicyStoreId,
[property: CliOption("--policy-id")] string PolicyId,
[property: CliOption("--definition")] string Definition
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}