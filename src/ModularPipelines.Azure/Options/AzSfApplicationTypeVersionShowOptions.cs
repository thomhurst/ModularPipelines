using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "application-type", "version", "show")]
public record AzSfApplicationTypeVersionShowOptions(
[property: CliOption("--application-type-name")] string ApplicationTypeName,
[property: CliOption("--application-type-version")] string ApplicationTypeVersion,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;