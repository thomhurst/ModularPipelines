using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "delete-configuration-set-event-destination")]
public record AwsSesv2DeleteConfigurationSetEventDestinationOptions(
[property: CliOption("--configuration-set-name")] string ConfigurationSetName,
[property: CliOption("--event-destination-name")] string EventDestinationName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}