using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("netappfiles", "volume", "revert")]
public record AzNetappfilesVolumeRevertOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--snapshot-id")] string SnapshotId
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}