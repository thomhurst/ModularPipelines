using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "update-wireless-gateway")]
public record AwsIotwirelessUpdateWirelessGatewayOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--join-eui-filters")]
    public string[]? JoinEuiFilters { get; set; }

    [CommandSwitch("--net-id-filters")]
    public string[]? NetIdFilters { get; set; }

    [CommandSwitch("--max-eirp")]
    public float? MaxEirp { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}