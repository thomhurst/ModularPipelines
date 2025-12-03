using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apprunner", "delete-vpc-ingress-connection")]
public record AwsApprunnerDeleteVpcIngressConnectionOptions(
[property: CliOption("--vpc-ingress-connection-arn")] string VpcIngressConnectionArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}