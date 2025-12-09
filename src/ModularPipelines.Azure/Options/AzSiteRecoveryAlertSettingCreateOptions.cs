using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("site-recovery", "alert-setting", "create")]
public record AzSiteRecoveryAlertSettingCreateOptions(
[property: CliOption("--alert-setting-name")] string AlertSettingName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--custom-email-addresses")]
    public string? CustomEmailAddresses { get; set; }

    [CliOption("--locale")]
    public string? Locale { get; set; }

    [CliOption("--send-to-owners")]
    public string? SendToOwners { get; set; }
}