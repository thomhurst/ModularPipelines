using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "start-multicast-group-session")]
public record AwsIotwirelessStartMulticastGroupSessionOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--lorawan")] string Lorawan
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}