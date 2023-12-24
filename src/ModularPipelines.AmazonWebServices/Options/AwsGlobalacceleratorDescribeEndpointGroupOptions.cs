using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "describe-endpoint-group")]
public record AwsGlobalacceleratorDescribeEndpointGroupOptions(
[property: CommandSwitch("--endpoint-group-arn")] string EndpointGroupArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}