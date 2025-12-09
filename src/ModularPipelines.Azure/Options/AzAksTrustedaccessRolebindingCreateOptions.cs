using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "trustedaccess", "rolebinding", "create")]
public record AzAksTrustedaccessRolebindingCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--roles")] string Roles,
[property: CliOption("--source-resource-id")] string SourceResourceId
) : AzOptions;