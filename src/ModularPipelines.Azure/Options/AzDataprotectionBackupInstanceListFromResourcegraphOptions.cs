using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-instance", "list-from-resourcegraph")]
public record AzDataprotectionBackupInstanceListFromResourcegraphOptions(
[property: CommandSwitch("--datasource-type")] string DatasourceType
) : AzOptions
{
    [CommandSwitch("--datasource-id")]
    public string? DatasourceId { get; set; }

    [CommandSwitch("--protection-status")]
    public string? ProtectionStatus { get; set; }

    [CommandSwitch("--resource-groups")]
    public string? ResourceGroups { get; set; }

    [CommandSwitch("--subscriptions")]
    public string? Subscriptions { get; set; }

    [CommandSwitch("--vaults")]
    public string? Vaults { get; set; }
}