using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "update-configuration-set-event-destination")]
public record AwsSesUpdateConfigurationSetEventDestinationOptions(
[property: CommandSwitch("--configuration-set-name")] string ConfigurationSetName,
[property: CommandSwitch("--event-destination")] string EventDestination
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}