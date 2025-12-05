using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "application", "delete")]
public record AzSfApplicationDeleteOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;