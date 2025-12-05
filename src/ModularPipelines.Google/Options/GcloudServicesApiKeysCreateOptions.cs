using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "api-keys", "create")]
public record GcloudServicesApiKeysCreateOptions : GcloudOptions
{
    [CliOption("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--key-id")]
    public string? KeyId { get; set; }

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