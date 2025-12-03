using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cache", "origins", "create")]
public record GcloudEdgeCacheOriginsCreateOptions(
[property: CliArgument] string Origin,
[property: CliArgument] string Location,
[property: CliOption("--origin-address")] string OriginAddress
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--failover-origin")]
    public string? FailoverOrigin { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--max-attempts")]
    public string? MaxAttempts { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--response-timeout")]
    public string? ResponseTimeout { get; set; }

    [CliOption("--retry-conditions")]
    public string[]? RetryConditions { get; set; }
}