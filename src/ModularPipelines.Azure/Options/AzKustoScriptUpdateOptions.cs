using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("kusto", "script", "update")]
public record AzKustoScriptUpdateOptions : AzOptions
{
    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliFlag("--continue-on-errors")]
    public bool? ContinueOnErrors { get; set; }

    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliFlag("--force-update-tag")]
    public bool? ForceUpdateTag { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--script-content")]
    public string? ScriptContent { get; set; }

    [CliOption("--script-url")]
    public string? ScriptUrl { get; set; }

    [CliOption("--script-url-sas-token")]
    public string? ScriptUrlSasToken { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}