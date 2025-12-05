using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("provider", "permission", "list")]
public record AzProviderPermissionListOptions(
[property: CliOption("--namespace")] string Namespace
) : AzOptions;