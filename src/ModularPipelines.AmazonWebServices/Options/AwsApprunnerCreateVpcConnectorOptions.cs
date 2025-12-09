using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apprunner", "create-vpc-connector")]
public record AwsApprunnerCreateVpcConnectorOptions(
[property: CliOption("--vpc-connector-name")] string VpcConnectorName,
[property: CliOption("--subnets")] string[] Subnets
) : AwsOptions
{
    [CliOption("--security-groups")]
    public string[]? SecurityGroups { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}