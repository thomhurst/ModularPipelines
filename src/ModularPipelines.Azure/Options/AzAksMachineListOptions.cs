using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "machine", "list")]
public record AzAksMachineListOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--nodepool-name")] string NodepoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;