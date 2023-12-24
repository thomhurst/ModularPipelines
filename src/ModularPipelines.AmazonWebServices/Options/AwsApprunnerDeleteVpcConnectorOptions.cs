using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apprunner", "delete-vpc-connector")]
public record AwsApprunnerDeleteVpcConnectorOptions(
[property: CommandSwitch("--vpc-connector-arn")] string VpcConnectorArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}