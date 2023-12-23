using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "associate-multicast-group-with-fuota-task")]
public record AwsIotwirelessAssociateMulticastGroupWithFuotaTaskOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--multicast-group-id")] string MulticastGroupId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}