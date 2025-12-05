using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivschat", "create-room")]
public record AwsIvschatCreateRoomOptions : AwsOptions
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

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}