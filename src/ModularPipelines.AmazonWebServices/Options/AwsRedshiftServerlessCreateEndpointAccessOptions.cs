using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "create-endpoint-access")]
public record AwsRedshiftServerlessCreateEndpointAccessOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--subnet-ids")] string[] SubnetIds,
[property: CliOption("--workgroup-name")] string WorkgroupName
) : AwsOptions
{
    [CliOption("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}