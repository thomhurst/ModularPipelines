using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-instance-connect-endpoint")]
public record AwsEc2DeleteInstanceConnectEndpointOptions(
[property: CommandSwitch("--instance-connect-endpoint-id")] string InstanceConnectEndpointId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}