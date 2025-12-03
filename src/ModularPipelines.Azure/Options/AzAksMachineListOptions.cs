using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "machine", "list")]
public record AzAksMachineListOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--nodepool-name")] string NodepoolName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;