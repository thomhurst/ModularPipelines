using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "settings", "disable-upgrade-redirection")]
public record GcloudArtifactsSettingsDisableUpgradeRedirectionOptions : GcloudOptions;