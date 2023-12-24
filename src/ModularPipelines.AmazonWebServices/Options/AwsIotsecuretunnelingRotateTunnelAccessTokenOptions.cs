using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsecuretunneling", "rotate-tunnel-access-token")]
public record AwsIotsecuretunnelingRotateTunnelAccessTokenOptions(
[property: CommandSwitch("--tunnel-id")] string TunnelId,
[property: CommandSwitch("--client-mode")] string ClientMode
) : AwsOptions
{
    [CommandSwitch("--destination-config")]
    public string? DestinationConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}