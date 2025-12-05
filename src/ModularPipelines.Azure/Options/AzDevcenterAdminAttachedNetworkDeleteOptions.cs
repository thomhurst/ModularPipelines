using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "admin", "attached-network", "delete")]
public record AzDevcenterAdminAttachedNetworkDeleteOptions : AzOptions
{
    [CliFlag("--attached-network-connection-name")]
    public bool? AttachedNetworkConnectionName { get; set; }

    [CliOption("--dev-center")]
    public string? DevCenter { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}