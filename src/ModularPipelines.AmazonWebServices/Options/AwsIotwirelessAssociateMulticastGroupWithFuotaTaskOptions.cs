using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "associate-multicast-group-with-fuota-task")]
public record AwsIotwirelessAssociateMulticastGroupWithFuotaTaskOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--multicast-group-id")] string MulticastGroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}