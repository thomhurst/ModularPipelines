using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "update-configuration-set-tracking-options")]
public record AwsSesUpdateConfigurationSetTrackingOptionsOptions(
[property: CliOption("--configuration-set-name")] string ConfigurationSetName,
[property: CliOption("--tracking-options")] string TrackingOptions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}