using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-application-type", "version", "show")]
public record AzSfManagedApplicationTypeVersionShowOptions(
[property: CommandSwitch("--application-type-name")] string ApplicationTypeName,
[property: CommandSwitch("--application-type-version")] string ApplicationTypeVersion,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;