using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "create-configuration-set-tracking-options")]
public record AwsSesCreateConfigurationSetTrackingOptionsOptions(
[property: CliOption("--configuration-set-name")] string ConfigurationSetName,
[property: CliOption("--tracking-options")] string TrackingOptions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}