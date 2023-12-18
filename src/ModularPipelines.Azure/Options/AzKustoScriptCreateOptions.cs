using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "script", "create")]
public record AzKustoScriptCreateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--continue-on-errors")]
    public bool? ContinueOnErrors { get; set; }

    [BooleanCommandSwitch("--force-update-tag")]
    public bool? ForceUpdateTag { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--script-content")]
    public string? ScriptContent { get; set; }

    [CommandSwitch("--script-url")]
    public string? ScriptUrl { get; set; }

    [CommandSwitch("--script-url-sas-token")]
    public string? ScriptUrlSasToken { get; set; }
}