using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice", "create-configuration-set-event-destination")]
public record AwsPinpointSmsVoiceCreateConfigurationSetEventDestinationOptions(
[property: CliOption("--configuration-set-name")] string ConfigurationSetName
) : AwsOptions
{
    [CliOption("--event-destination")]
    public string? EventDestination { get; set; }

    [CliOption("--event-destination-name")]
    public string? EventDestinationName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}