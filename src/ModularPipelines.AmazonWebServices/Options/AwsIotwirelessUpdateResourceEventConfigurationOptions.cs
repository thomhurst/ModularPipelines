using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "update-resource-event-configuration")]
public record AwsIotwirelessUpdateResourceEventConfigurationOptions(
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--identifier-type")] string IdentifierType
) : AwsOptions
{
    [CliOption("--partner-type")]
    public string? PartnerType { get; set; }

    [CliOption("--device-registration-state")]
    public string? DeviceRegistrationState { get; set; }

    [CliOption("--proximity")]
    public string? Proximity { get; set; }

    [CliOption("--join")]
    public string? Join { get; set; }

    [CliOption("--connection-status")]
    public string? ConnectionStatus { get; set; }

    [CliOption("--message-delivery-status")]
    public string? MessageDeliveryStatus { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}