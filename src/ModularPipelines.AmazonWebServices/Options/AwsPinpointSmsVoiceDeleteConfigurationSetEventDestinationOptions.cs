using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice", "delete-configuration-set-event-destination")]
public record AwsPinpointSmsVoiceDeleteConfigurationSetEventDestinationOptions(
[property: CommandSwitch("--configuration-set-name")] string ConfigurationSetName,
[property: CommandSwitch("--event-destination-name")] string EventDestinationName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}