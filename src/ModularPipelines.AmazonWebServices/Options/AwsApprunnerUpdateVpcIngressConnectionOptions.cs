using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apprunner", "update-vpc-ingress-connection")]
public record AwsApprunnerUpdateVpcIngressConnectionOptions(
[property: CommandSwitch("--vpc-ingress-connection-arn")] string VpcIngressConnectionArn,
[property: CommandSwitch("--ingress-vpc-configuration")] string IngressVpcConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}