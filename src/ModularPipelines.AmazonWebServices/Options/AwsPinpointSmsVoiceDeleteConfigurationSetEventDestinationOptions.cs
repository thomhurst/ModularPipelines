using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice", "delete-configuration-set-event-destination")]
public record AwsPinpointSmsVoiceDeleteConfigurationSetEventDestinationOptions(
[property: CliOption("--configuration-set-name")] string ConfigurationSetName,
[property: CliOption("--event-destination-name")] string EventDestinationName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}