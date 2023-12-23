using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsecuretunneling", "describe-tunnel")]
public record AwsIotsecuretunnelingDescribeTunnelOptions(
[property: CommandSwitch("--tunnel-id")] string TunnelId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}