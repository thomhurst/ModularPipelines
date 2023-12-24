using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "start-multicast-group-session")]
public record AwsIotwirelessStartMulticastGroupSessionOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--lorawan")] string Lorawan
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}