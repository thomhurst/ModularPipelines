using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("recaptcha", "keys", "create")]
public record GcloudRecaptchaKeysCreateOptions(
[property: CommandSwitch("--display-name")] string DisplayName,
[property: BooleanCommandSwitch("--android")] bool Android,
[property: BooleanCommandSwitch("--support-non-google-app-store-distribution")] bool SupportNonGoogleAppStoreDistribution,
[property: BooleanCommandSwitch("--allow-all-package-names")] bool AllowAllPackageNames,
[property: CommandSwitch("--package-names")] string[] PackageNames,
[property: BooleanCommandSwitch("--ios")] bool Ios,
[property: BooleanCommandSwitch("--allow-all-bundle-ids")] bool AllowAllBundleIds,
[property: CommandSwitch("--bundle-ids")] string[] BundleIds,
[property: CommandSwitch("--key-id")] string KeyId,
[property: CommandSwitch("--private-key-file")] string PrivateKeyFile,
[property: CommandSwitch("--team-id")] string TeamId,
[property: BooleanCommandSwitch("--web")] bool Web,
[property: BooleanCommandSwitch("--allow-amp-traffic")] bool AllowAmpTraffic,
[property: CommandSwitch("--integration-type")] string IntegrationType,
[property: BooleanCommandSwitch("checkbox")] bool Checkbox,
[property: BooleanCommandSwitch("invisible")] bool Invisible,
[property: BooleanCommandSwitch("score")] bool Score,
[property: CommandSwitch("--security-preference")] string SecurityPreference,
[property: CommandSwitch("--testing-challenge")] string TestingChallenge,
[property: BooleanCommandSwitch("challenge")] bool Challenge,
[property: BooleanCommandSwitch("nocaptcha")] bool Nocaptcha,
[property: BooleanCommandSwitch("--allow-all-domains")] bool AllowAllDomains,
[property: CommandSwitch("--domains")] string[] Domains
) : GcloudOptions
{
    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--testing-score")]
    public string? TestingScore { get; set; }

    [CommandSwitch("--waf-feature")]
    public string? WafFeature { get; set; }

    [BooleanCommandSwitch("action-token")]
    public bool? ActionToken { get; set; }

    [BooleanCommandSwitch("challenge-page")]
    public bool? ChallengePage { get; set; }

    [BooleanCommandSwitch("express")]
    public bool? Express { get; set; }

    [BooleanCommandSwitch("session-token")]
    public bool? SessionToken { get; set; }

    [CommandSwitch("--waf-service")]
    public string? WafService { get; set; }

    [BooleanCommandSwitch("ca")]
    public bool? Ca { get; set; }

    [BooleanCommandSwitch("cloudflare")]
    public bool? Cloudflare { get; set; }

    [BooleanCommandSwitch("fastly")]
    public bool? Fastly { get; set; }

    [BooleanCommandSwitch("unspecified")]
    public bool? Unspecified { get; set; }
}