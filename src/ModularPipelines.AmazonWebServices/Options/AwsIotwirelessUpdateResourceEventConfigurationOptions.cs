using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "update-resource-event-configuration")]
public record AwsIotwirelessUpdateResourceEventConfigurationOptions(
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--identifier-type")] string IdentifierType
) : AwsOptions
{
    [CommandSwitch("--partner-type")]
    public string? PartnerType { get; set; }

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