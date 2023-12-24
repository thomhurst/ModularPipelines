using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ivschat", "update-room")]
public record AwsIvschatUpdateRoomOptions(
[property: CommandSwitch("--identifier")] string Identifier
) : AwsOptions
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

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}