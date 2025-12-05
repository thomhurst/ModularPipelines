using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("verifiedpermissions", "create-policy")]
public record AwsVerifiedpermissionsCreatePolicyOptions(
[property: CliOption("--policy-store-id")] string PolicyStoreId,
[property: CliOption("--definition")] string Definition
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}