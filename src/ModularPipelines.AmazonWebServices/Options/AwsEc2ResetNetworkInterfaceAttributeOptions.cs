using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "reset-network-interface-attribute")]
public record AwsEc2ResetNetworkInterfaceAttributeOptions(
[property: CommandSwitch("--network-interface-id")] string NetworkInterfaceId
) : AwsOptions
{
    [CommandSwitch("--source-dest-check")]
    public string? SourceDestCheck { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}