using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cache", "origins", "create")]
public record GcloudEdgeCacheOriginsCreateOptions(
[property: PositionalArgument] string Origin,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--origin-address")] string OriginAddress
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--failover-origin")]
    public string? FailoverOrigin { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--max-attempts")]
    public string? MaxAttempts { get; set; }

    [CommandSwitch("--port")]
    public string? Port { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--response-timeout")]
    public string? ResponseTimeout { get; set; }

    [CommandSwitch("--retry-conditions")]
    public string[]? RetryConditions { get; set; }
}