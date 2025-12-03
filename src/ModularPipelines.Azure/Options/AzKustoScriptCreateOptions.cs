using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kusto", "script", "create")]
public record AzKustoScriptCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--continue-on-errors")]
    public bool? ContinueOnErrors { get; set; }

    [CliFlag("--force-update-tag")]
    public bool? ForceUpdateTag { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--script-content")]
    public string? ScriptContent { get; set; }

    [CliOption("--script-url")]
    public string? ScriptUrl { get; set; }

    [CliOption("--script-url-sas-token")]
    public string? ScriptUrlSasToken { get; set; }
}