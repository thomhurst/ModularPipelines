using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "app", "permission", "add")]
public record AzAdAppPermissionAddOptions(
[property: CliOption("--api")] string Api,
[property: CliOption("--api-permissions")] string ApiPermissions,
[property: CliOption("--id")] string Id
) : AzOptions;