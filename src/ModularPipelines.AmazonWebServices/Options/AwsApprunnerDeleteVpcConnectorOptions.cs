using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apprunner", "delete-vpc-connector")]
public record AwsApprunnerDeleteVpcConnectorOptions(
[property: CliOption("--vpc-connector-arn")] string VpcConnectorArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}