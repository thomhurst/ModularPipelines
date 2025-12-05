using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivschat", "update-room")]
public record AwsIvschatUpdateRoomOptions(
[property: CliOption("--identifier")] string Identifier
) : AwsOptions
{
    [CliOption("--logging-configuration-identifiers")]
    public string[]? LoggingConfigurationIdentifiers { get; set; }

    [CliOption("--maximum-message-length")]
    public int? MaximumMessageLength { get; set; }

    [CliOption("--maximum-message-rate-per-second")]
    public int? MaximumMessageRatePerSecond { get; set; }

    [CliOption("--message-review-handler")]
    public string? MessageReviewHandler { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}