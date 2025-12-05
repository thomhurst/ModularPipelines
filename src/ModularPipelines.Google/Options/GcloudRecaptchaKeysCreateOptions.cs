using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recaptcha", "keys", "create")]
public record GcloudRecaptchaKeysCreateOptions(
[property: CliOption("--display-name")] string DisplayName,
[property: CliFlag("--android")] bool Android,
[property: CliFlag("--support-non-google-app-store-distribution")] bool SupportNonGoogleAppStoreDistribution,
[property: CliFlag("--allow-all-package-names")] bool AllowAllPackageNames,
[property: CliOption("--package-names")] string[] PackageNames,
[property: CliFlag("--ios")] bool Ios,
[property: CliFlag("--allow-all-bundle-ids")] bool AllowAllBundleIds,
[property: CliOption("--bundle-ids")] string[] BundleIds,
[property: CliOption("--key-id")] string KeyId,
[property: CliOption("--private-key-file")] string PrivateKeyFile,
[property: CliOption("--team-id")] string TeamId,
[property: CliFlag("--web")] bool Web,
[property: CliFlag("--allow-amp-traffic")] bool AllowAmpTraffic,
[property: CliOption("--integration-type")] string IntegrationType,
[property: CliFlag("checkbox")] bool Checkbox,
[property: CliFlag("invisible")] bool Invisible,
[property: CliFlag("score")] bool Score,
[property: CliOption("--security-preference")] string SecurityPreference,
[property: CliOption("--testing-challenge")] string TestingChallenge,
[property: CliFlag("challenge")] bool Challenge,
[property: CliFlag("nocaptcha")] bool Nocaptcha,
[property: CliFlag("--allow-all-domains")] bool AllowAllDomains,
[property: CliOption("--domains")] string[] Domains
) : GcloudOptions
{
    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--testing-score")]
    public string? TestingScore { get; set; }

    [CliOption("--waf-feature")]
    public string? WafFeature { get; set; }

    [CliFlag("action-token")]
    public bool? ActionToken { get; set; }

    [CliFlag("challenge-page")]
    public bool? ChallengePage { get; set; }

    [CliFlag("express")]
    public bool? Express { get; set; }

    [CliFlag("session-token")]
    public bool? SessionToken { get; set; }

    [CliOption("--waf-service")]
    public string? WafService { get; set; }

    [CliFlag("ca")]
    public bool? Ca { get; set; }

    [CliFlag("cloudflare")]
    public bool? Cloudflare { get; set; }

    [CliFlag("fastly")]
    public bool? Fastly { get; set; }

    [CliFlag("unspecified")]
    public bool? Unspecified { get; set; }
}