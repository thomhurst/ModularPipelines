using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "create-publishing-destination")]
public record AwsGuarddutyCreatePublishingDestinationOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--destination-type")] string DestinationType,
[property: CliOption("--destination-properties")] string DestinationProperties
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}