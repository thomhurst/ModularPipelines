using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recaptcha", "keys", "update")]
public record GcloudRecaptchaKeysUpdateOptions(
[property: CliArgument] string Key
) : GcloudOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliFlag("--android")]
    public bool? Android { get; set; }

    [CliFlag("--allow-all-package-names")]
    public bool? AllowAllPackageNames { get; set; }

    [CliOption("--package-names")]
    public string[]? PackageNames { get; set; }

    [CliFlag("--ios")]
    public bool? Ios { get; set; }

    [CliFlag("--allow-all-bundle-ids")]
    public bool? AllowAllBundleIds { get; set; }

    [CliOption("--bundle-ids")]
    public string[]? BundleIds { get; set; }

    [CliOption("--key-id")]
    public string? KeyId { get; set; }

    [CliOption("--private-key-file")]
    public string? PrivateKeyFile { get; set; }

    [CliOption("--team-id")]
    public string? TeamId { get; set; }

    [CliFlag("--web")]
    public bool? Web { get; set; }

    [CliFlag("--allow-amp-traffic")]
    public bool? AllowAmpTraffic { get; set; }

    [CliOption("--security-preference")]
    public string? SecurityPreference { get; set; }

    [CliFlag("--allow-all-domains")]
    public bool? AllowAllDomains { get; set; }

    [CliOption("--domains")]
    public string[]? Domains { get; set; }
}