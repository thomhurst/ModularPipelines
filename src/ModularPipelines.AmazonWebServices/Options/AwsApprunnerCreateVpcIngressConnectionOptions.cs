using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apprunner", "create-vpc-ingress-connection")]
public record AwsApprunnerCreateVpcIngressConnectionOptions(
[property: CliOption("--service-arn")] string ServiceArn,
[property: CliOption("--vpc-ingress-connection-name")] string VpcIngressConnectionName,
[property: CliOption("--ingress-vpc-configuration")] string IngressVpcConfiguration
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}