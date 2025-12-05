using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune-graph", "create-private-graph-endpoint")]
public record AwsNeptuneGraphCreatePrivateGraphEndpointOptions(
[property: CliOption("--graph-identifier")] string GraphIdentifier
) : AwsOptions
{
    [CliOption("--vpc-id")]
    public string? VpcId { get; set; }

    [CliOption("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}