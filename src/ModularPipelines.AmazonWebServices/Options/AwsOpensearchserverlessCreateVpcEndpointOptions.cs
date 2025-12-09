using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearchserverless", "create-vpc-endpoint")]
public record AwsOpensearchserverlessCreateVpcEndpointOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--subnet-ids")] string[] SubnetIds,
[property: CliOption("--vpc-id")] string VpcId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}