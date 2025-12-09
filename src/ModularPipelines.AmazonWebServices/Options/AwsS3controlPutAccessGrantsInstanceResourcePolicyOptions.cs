using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "put-access-grants-instance-resource-policy")]
public record AwsS3controlPutAccessGrantsInstanceResourcePolicyOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--policy")] string Policy
) : AwsOptions
{
    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}