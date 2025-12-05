using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsecuretunneling", "rotate-tunnel-access-token")]
public record AwsIotsecuretunnelingRotateTunnelAccessTokenOptions(
[property: CliOption("--tunnel-id")] string TunnelId,
[property: CliOption("--client-mode")] string ClientMode
) : AwsOptions
{
    [CliOption("--destination-config")]
    public string? DestinationConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}