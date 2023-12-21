using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "snapshot", "policy", "volumes")]
public record AzNetappfilesSnapshotPolicyVolumesOptions : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--snapshot-policy-name")]
    public string? SnapshotPolicyName { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}