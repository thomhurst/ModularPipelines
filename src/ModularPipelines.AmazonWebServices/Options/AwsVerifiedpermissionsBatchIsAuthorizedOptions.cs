using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("verifiedpermissions", "batch-is-authorized")]
public record AwsVerifiedpermissionsBatchIsAuthorizedOptions(
[property: CliOption("--policy-store-id")] string PolicyStoreId,
[property: CliOption("--requests")] string[] Requests
) : AwsOptions
{
    [CliOption("--entities")]
    public string? Entities { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}