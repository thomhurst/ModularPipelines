using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apprunner", "update-vpc-ingress-connection")]
public record AwsApprunnerUpdateVpcIngressConnectionOptions(
[property: CliOption("--vpc-ingress-connection-arn")] string VpcIngressConnectionArn,
[property: CliOption("--ingress-vpc-configuration")] string IngressVpcConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}