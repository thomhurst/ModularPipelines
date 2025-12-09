using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("tsi", "event-source", "iothub", "update")]
public record AzTsiEventSourceIothubUpdateOptions : AzOptions
{
    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliOption("--event-source-name")]
    public string? EventSourceName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--local-timestamp")]
    public string? LocalTimestamp { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--shared-access-key")]
    public string? SharedAccessKey { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--timestamp-property-name")]
    public string? TimestampPropertyName { get; set; }
}