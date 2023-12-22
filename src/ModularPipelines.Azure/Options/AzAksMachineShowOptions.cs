using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "machine", "show")]
public record AzAksMachineShowOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--machine-name")] string MachineName,
[property: CommandSwitch("--nodepool-name")] string NodepoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;