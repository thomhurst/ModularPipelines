using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-email", "create-configuration-set")]
public record AwsPinpointEmailCreateConfigurationSetOptions(
[property: CommandSwitch("--configuration-set-name")] string ConfigurationSetName
) : AwsOptions
{
    [CommandSwitch("--tracking-options")]
    public string? TrackingOptions { get; set; }

    [CommandSwitch("--delivery-options")]
    public string? DeliveryOptions { get; set; }

    [CommandSwitch("--reputation-options")]
    public string? ReputationOptions { get; set; }

    [CommandSwitch("--sending-options")]
    public string? SendingOptions { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}