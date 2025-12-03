using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "api-keys", "update")]
public record GcloudServicesApiKeysUpdateOptions(
[property: CliArgument] string Key,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [CliFlag("--clear-annotations")]
    public bool? ClearAnnotations { get; set; }

    [CliFlag("--clear-restrictions")]
    public bool? ClearRestrictions { get; set; }

    [CliOption("--api-target")]
    public string[]? ApiTarget { get; set; }

    [CliOption("--allowed-application")]
    public string[]? AllowedApplication { get; set; }

    [CliOption("--allowed-bundle-ids")]
    public string[]? AllowedBundleIds { get; set; }

    [CliOption("--allowed-ips")]
    public string[]? AllowedIps { get; set; }

    [CliOption("--allowed-referrers")]
    public string[]? AllowedReferrers { get; set; }
}