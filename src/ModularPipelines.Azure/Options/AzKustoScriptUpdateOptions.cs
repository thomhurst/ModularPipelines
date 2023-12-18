using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "script", "update")]
public record AzKustoScriptUpdateOptions : AzOptions
{
    [CommandSwitch("--cluster-name")]
    public string? ClusterName { get; set; }

    [BooleanCommandSwitch("--continue-on-errors")]
    public bool? ContinueOnErrors { get; set; }

    [CommandSwitch("--database-name")]
    public string? DatabaseName { get; set; }

    [BooleanCommandSwitch("--force-update-tag")]
    public bool? ForceUpdateTag { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--script-content")]
    public string? ScriptContent { get; set; }

    [CommandSwitch("--script-url")]
    public string? ScriptUrl { get; set; }

    [CommandSwitch("--script-url-sas-token")]
    public string? ScriptUrlSasToken { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}