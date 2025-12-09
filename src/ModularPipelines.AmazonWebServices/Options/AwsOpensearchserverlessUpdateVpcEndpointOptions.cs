using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearchserverless", "update-vpc-endpoint")]
public record AwsOpensearchserverlessUpdateVpcEndpointOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--add-security-group-ids")]
    public string[]? AddSecurityGroupIds { get; set; }

    [CliOption("--add-subnet-ids")]
    public string[]? AddSubnetIds { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--remove-security-group-ids")]
    public string[]? RemoveSecurityGroupIds { get; set; }

    [CliOption("--remove-subnet-ids")]
    public string[]? RemoveSubnetIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}