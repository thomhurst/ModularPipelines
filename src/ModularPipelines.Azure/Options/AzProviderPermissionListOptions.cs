using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("provider", "permission", "list")]
public record AzProviderPermissionListOptions(
[property: CliOption("--namespace")] string Namespace
) : AzOptions;