using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "trustedaccess", "rolebinding", "create")]
public record AzAksTrustedaccessRolebindingCreateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--roles")] string Roles,
[property: CommandSwitch("--source-resource-id")] string SourceResourceId
) : AzOptions;