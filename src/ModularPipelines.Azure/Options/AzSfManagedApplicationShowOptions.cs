using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "managed-application", "show")]
public record AzSfManagedApplicationShowOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;