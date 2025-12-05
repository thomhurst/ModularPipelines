using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsecuretunneling", "open-tunnel")]
public record AwsIotsecuretunnelingOpenTunnelOptions : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--destination-config")]
    public string? DestinationConfig { get; set; }

    [CliOption("--timeout-config")]
    public string? TimeoutConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}