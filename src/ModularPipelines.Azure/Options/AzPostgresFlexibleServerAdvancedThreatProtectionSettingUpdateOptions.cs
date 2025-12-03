using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("postgres", "flexible-server", "advanced-threat-protection-setting", "update")]
public record AzPostgresFlexibleServerAdvancedThreatProtectionSettingUpdateOptions(
[property: CliOption("--state")] string State
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server-name")]
    public string? ServerName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}