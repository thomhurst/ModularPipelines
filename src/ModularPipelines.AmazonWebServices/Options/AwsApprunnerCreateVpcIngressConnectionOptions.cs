using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apprunner", "create-vpc-ingress-connection")]
public record AwsApprunnerCreateVpcIngressConnectionOptions(
[property: CommandSwitch("--service-arn")] string ServiceArn,
[property: CommandSwitch("--vpc-ingress-connection-name")] string VpcIngressConnectionName,
[property: CommandSwitch("--ingress-vpc-configuration")] string IngressVpcConfiguration
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}