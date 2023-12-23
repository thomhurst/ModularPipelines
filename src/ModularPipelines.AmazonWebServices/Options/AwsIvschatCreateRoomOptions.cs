using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ivschat", "create-room")]
public record AwsIvschatCreateRoomOptions : AwsOptions
{
    [CommandSwitch("--logging-configuration-identifiers")]
    public string[]? LoggingConfigurationIdentifiers { get; set; }

    [CommandSwitch("--maximum-message-length")]
    public int? MaximumMessageLength { get; set; }

    [CommandSwitch("--maximum-message-rate-per-second")]
    public int? MaximumMessageRatePerSecond { get; set; }

    [CommandSwitch("--message-review-handler")]
    public string? MessageReviewHandler { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}