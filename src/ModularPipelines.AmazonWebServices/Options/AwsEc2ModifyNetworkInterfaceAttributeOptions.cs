using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-network-interface-attribute")]
public record AwsEc2ModifyNetworkInterfaceAttributeOptions(
[property: CommandSwitch("--network-interface-id")] string NetworkInterfaceId
) : AwsOptions
{
    [CommandSwitch("--attachment")]
    public string? Attachment { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--groups")]
    public string[]? Groups { get; set; }

    [CommandSwitch("--ena-srd-specification")]
    public string? EnaSrdSpecification { get; set; }

    [CommandSwitch("--connection-tracking-specification")]
    public string? ConnectionTrackingSpecification { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}