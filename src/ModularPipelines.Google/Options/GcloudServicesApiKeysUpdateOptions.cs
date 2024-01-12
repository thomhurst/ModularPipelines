using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("services", "api-keys", "update")]
public record GcloudServicesApiKeysUpdateOptions(
[property: PositionalArgument] string Key,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [BooleanCommandSwitch("--clear-annotations")]
    public bool? ClearAnnotations { get; set; }

    [BooleanCommandSwitch("--clear-restrictions")]
    public bool? ClearRestrictions { get; set; }

    [CommandSwitch("--api-target")]
    public string[]? ApiTarget { get; set; }

    [CommandSwitch("--allowed-application")]
    public string[]? AllowedApplication { get; set; }

    [CommandSwitch("--allowed-bundle-ids")]
    public string[]? AllowedBundleIds { get; set; }

    [CommandSwitch("--allowed-ips")]
    public string[]? AllowedIps { get; set; }

    [CommandSwitch("--allowed-referrers")]
    public string[]? AllowedReferrers { get; set; }
}