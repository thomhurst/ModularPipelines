using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "disassociate-multicast-group-from-fuota-task")]
public record AwsIotwirelessDisassociateMulticastGroupFromFuotaTaskOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--multicast-group-id")] string MulticastGroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}