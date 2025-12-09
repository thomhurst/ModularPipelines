using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "update-wireless-gateway")]
public record AwsIotwirelessUpdateWirelessGatewayOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--join-eui-filters")]
    public string[]? JoinEuiFilters { get; set; }

    [CliOption("--net-id-filters")]
    public string[]? NetIdFilters { get; set; }

    [CliOption("--max-eirp")]
    public float? MaxEirp { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}