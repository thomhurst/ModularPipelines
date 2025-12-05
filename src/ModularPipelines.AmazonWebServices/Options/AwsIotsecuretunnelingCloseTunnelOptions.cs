using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsecuretunneling", "close-tunnel")]
public record AwsIotsecuretunnelingCloseTunnelOptions(
[property: CliOption("--tunnel-id")] string TunnelId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}