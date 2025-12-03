using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devcenter", "admin", "attached-network", "show")]
public record AzDevcenterAdminAttachedNetworkShowOptions : AzOptions
{
    [CliFlag("--attached-network-connection-name")]
    public bool? AttachedNetworkConnectionName { get; set; }

    [CliOption("--dev-center")]
    public string? DevCenter { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}