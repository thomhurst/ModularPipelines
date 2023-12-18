using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("site-recovery", "alert-setting", "create")]
public record AzSiteRecoveryAlertSettingCreateOptions(
[property: CommandSwitch("--alert-setting-name")] string AlertSettingName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--custom-email-addresses")]
    public string? CustomEmailAddresses { get; set; }

    [CommandSwitch("--locale")]
    public string? Locale { get; set; }

    [CommandSwitch("--send-to-owners")]
    public string? SendToOwners { get; set; }
}