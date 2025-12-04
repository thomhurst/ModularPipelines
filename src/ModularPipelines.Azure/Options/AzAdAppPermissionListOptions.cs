using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "app", "permission", "list")]
public record AzAdAppPermissionListOptions(
[property: CliOption("--id")] string Id
) : AzOptions;