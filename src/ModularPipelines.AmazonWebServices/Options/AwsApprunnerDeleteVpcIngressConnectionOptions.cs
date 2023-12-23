using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apprunner", "delete-vpc-ingress-connection")]
public record AwsApprunnerDeleteVpcIngressConnectionOptions(
[property: CommandSwitch("--vpc-ingress-connection-arn")] string VpcIngressConnectionArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}