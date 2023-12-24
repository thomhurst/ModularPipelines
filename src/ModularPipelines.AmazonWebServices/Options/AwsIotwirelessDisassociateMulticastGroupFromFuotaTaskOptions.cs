using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "disassociate-multicast-group-from-fuota-task")]
public record AwsIotwirelessDisassociateMulticastGroupFromFuotaTaskOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--multicast-group-id")] string MulticastGroupId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}