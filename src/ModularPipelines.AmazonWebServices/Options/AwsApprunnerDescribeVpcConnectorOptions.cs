using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apprunner", "describe-vpc-connector")]
public record AwsApprunnerDescribeVpcConnectorOptions(
[property: CommandSwitch("--vpc-connector-arn")] string VpcConnectorArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}