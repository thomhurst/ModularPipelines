using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "machine", "show")]
public record AzAksMachineShowOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--machine-name")] string MachineName,
[property: CliOption("--nodepool-name")] string NodepoolName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;