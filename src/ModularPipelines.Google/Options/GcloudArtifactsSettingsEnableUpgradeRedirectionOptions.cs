using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "settings", "enable-upgrade-redirection")]
public record GcloudArtifactsSettingsEnableUpgradeRedirectionOptions : GcloudOptions
{
    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }
}