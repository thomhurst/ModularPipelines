using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-application-type", "list")]
public record AzSfManagedApplicationTypeListOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;