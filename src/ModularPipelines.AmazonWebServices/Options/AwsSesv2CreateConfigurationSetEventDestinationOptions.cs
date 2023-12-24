using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "create-configuration-set-event-destination")]
public record AwsSesv2CreateConfigurationSetEventDestinationOptions(
[property: CommandSwitch("--configuration-set-name")] string ConfigurationSetName,
[property: CommandSwitch("--event-destination-name")] string EventDestinationName,
[property: CommandSwitch("--event-destination")] string EventDestination
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}