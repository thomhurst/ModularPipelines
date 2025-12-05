using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("netappfiles", "snapshot", "policy", "show")]
public record AzNetappfilesSnapshotPolicyShowOptions : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--snapshot-policy-name")]
    public string? SnapshotPolicyName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}