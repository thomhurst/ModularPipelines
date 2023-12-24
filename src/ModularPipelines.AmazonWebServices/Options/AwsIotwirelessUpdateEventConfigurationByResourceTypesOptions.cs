using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "update-event-configuration-by-resource-types")]
public record AwsIotwirelessUpdateEventConfigurationByResourceTypesOptions : AwsOptions
{
    [CommandSwitch("--device-registration-state")]
    public string? DeviceRegistrationState { get; set; }

    [CommandSwitch("--proximity")]
    public string? Proximity { get; set; }

    [CommandSwitch("--join")]
    public string? Join { get; set; }

    [CommandSwitch("--connection-status")]
    public string? ConnectionStatus { get; set; }

    [CommandSwitch("--message-delivery-status")]
    public string? MessageDeliveryStatus { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}