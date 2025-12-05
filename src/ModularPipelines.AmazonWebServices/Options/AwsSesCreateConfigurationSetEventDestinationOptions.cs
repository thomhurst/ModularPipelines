using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "create-configuration-set-event-destination")]
public record AwsSesCreateConfigurationSetEventDestinationOptions(
[property: CliOption("--configuration-set-name")] string ConfigurationSetName,
[property: CliOption("--event-destination")] string EventDestination
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}