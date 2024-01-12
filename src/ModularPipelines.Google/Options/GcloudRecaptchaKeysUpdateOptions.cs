using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("recaptcha", "keys", "update")]
public record GcloudRecaptchaKeysUpdateOptions(
[property: PositionalArgument] string Key
) : GcloudOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [BooleanCommandSwitch("--android")]
    public bool? Android { get; set; }

    [BooleanCommandSwitch("--allow-all-package-names")]
    public bool? AllowAllPackageNames { get; set; }

    [CommandSwitch("--package-names")]
    public string[]? PackageNames { get; set; }

    [BooleanCommandSwitch("--ios")]
    public bool? Ios { get; set; }

    [BooleanCommandSwitch("--allow-all-bundle-ids")]
    public bool? AllowAllBundleIds { get; set; }

    [CommandSwitch("--bundle-ids")]
    public string[]? BundleIds { get; set; }

    [CommandSwitch("--key-id")]
    public string? KeyId { get; set; }

    [CommandSwitch("--private-key-file")]
    public string? PrivateKeyFile { get; set; }

    [CommandSwitch("--team-id")]
    public string? TeamId { get; set; }

    [BooleanCommandSwitch("--web")]
    public bool? Web { get; set; }

    [BooleanCommandSwitch("--allow-amp-traffic")]
    public bool? AllowAmpTraffic { get; set; }

    [CommandSwitch("--security-preference")]
    public string? SecurityPreference { get; set; }

    [BooleanCommandSwitch("--allow-all-domains")]
    public bool? AllowAllDomains { get; set; }

    [CommandSwitch("--domains")]
    public string[]? Domains { get; set; }
}