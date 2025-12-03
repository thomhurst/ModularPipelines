using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles", "snapshot", "list")]
public record AzNetappfilesSnapshotListOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--volume-name")] string VolumeName
) : AzOptions;